using Newtonsoft.Json;
using TCPLocal.Server.Controllers;
using TCPLocal.Server.Helper;
using TCPLocal.Server.Models;

namespace TCPLocal
{
    public partial class MainView : Form
    {
        private TcpServerController _serverController;
        private bool resizingColumns = false;
        private Thread _serverThread;

        public MainView()
        {
            InitializeComponent();
            InitializeListView();
        }

        private void MainView_Load(object sender, EventArgs e)
        {
            StartServer();
        }

        private void InitializeListView()
        {
            // Assuming listView1 is your ListView control on the form
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;

            // Add columns to ListView
            listView1.Columns.Add("Guid");
            listView1.Columns.Add("Machine");
            listView1.Columns.Add("Username");
            listView1.Columns.Add("Domain");
            listView1.Columns.Add("OS");
            listView1.Columns.Add("Is64BitOS");
            listView1.Columns.Add("Local IP");
            listView1.Columns.Add("Remote IP");

            // Handle column width changes to ensure columns fill the available width
            listView1.ColumnWidthChanged += ListView_ColumnWidthChanged;
        }

        private void ListView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            // Only perform resizing if it's not already in progress
            if (!resizingColumns)
            {
                resizingColumns = true;
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                resizingColumns = false;
            }
        }

        private void StartServer()
        {
            try
            {
                var config = ConfigurationHelper.LoadConfig("config.yaml");
                _serverController = new TcpServerController(config.Server, AddClientToListView, RemoveClientFromListView);

                _serverThread = new Thread(new ThreadStart(_serverController.Start))
                {
                    IsBackground = true
                };
                _serverThread.Start();
                dateiToolStripMenuItem.Text = "Server Status: ON";
            }
            catch (Exception ex)
            {
                // Log the exception details
                MessageBox.Show($"Failed to start the server: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResizeListViewColumns()
        {
            // Only perform resizing if it's not already in progress
            if (!resizingColumns)
            {
                resizingColumns = true;
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                resizingColumns = false;
            }
        }

        private void AddClientToListView(string jsonData)
        {
            if (listView1.InvokeRequired)
            {
                listView1.Invoke(new Action<string>(AddClientToListView), jsonData);
            }
            else
            {
                try
                {
                    // Deserialize JSON into client model
                    var clientInfo = JsonConvert.DeserializeObject<ClientRemoteData>(jsonData);

                    // Create a ListViewItem with subitems
                    ListViewItem listViewItem = new ListViewItem(clientInfo.Guid.ToString());
                    listViewItem.SubItems.Add(clientInfo.MachineName);
                    listViewItem.SubItems.Add(clientInfo.UserName);
                    listViewItem.SubItems.Add(clientInfo.DomainName);
                    listViewItem.SubItems.Add(clientInfo.OperatingSystem);
                    listViewItem.SubItems.Add(clientInfo.Is64BitOs.ToString());
                    listViewItem.SubItems.Add(clientInfo.LocalIp);
                    listViewItem.SubItems.Add(clientInfo.ExternalIp);

                    // Add the ListViewItem to the ListView
                    listView1.Items.Add(listViewItem);
                    ResizeListViewColumns();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error adding client to ListView: {ex.Message}");
                }
            }
        }

        private void RemoveClientFromListView(string clientGuid)
        {
            if (listView1.InvokeRequired)
            {
                listView1.Invoke(new Action<string>(RemoveClientFromListView), clientGuid);
            }
            else
            {
                try
                {
                    string trimmedGuid = clientGuid.Trim('"');
                    ListViewItem itemToRemove = null!;

                    foreach (ListViewItem item in listView1.Items)
                    {
                        // Check both quoted and unquoted GUIDs
                        if (item.Text == trimmedGuid || item.Text.Trim('"') == trimmedGuid)
                        {
                            itemToRemove = item;
                            break;
                        }
                    }

                    if (itemToRemove != null)
                    {
                        listView1.Items.Remove(itemToRemove);
                        ResizeListViewColumns();
                    }
                    else
                    {
                        Console.WriteLine($"Client with GUID {clientGuid} not found in ListView.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error removing client from ListView: {ex.Message}");
                }
            }
        }

        private void MainView_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopServer();
        }

        private void StopServer()
        {
            if (_serverController != null)
            {
                _serverController.Stop();
            }

            if (_serverThread != null && _serverThread.IsAlive)
            {
                // Wait for the server thread to finish
                _serverThread.Join();
            }
        }
    }
}
