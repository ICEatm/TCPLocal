⚠️ Still in work

## 📡 TCPLocal - Basic Client & Server Program

This is a simple TCP client-server communication program where the client sends system information to the server at a set interval.

![Example Image](https://i.imgur.com/xSqnDmj.png)

### Features

- **Client-Server Communication**: Establishes a TCP connection between a client and server.
- **Data Exchange**: Sends system information from the client to the server, including:
  - UserName
  - MachineName
  - External IP
  - Local IP
  - DomainName
  - Operating System
  - Is OS 64 Bit

### How It Works

The server listens for incoming connections from clients. Upon connection, the client sends its system information to the server periodically. The server receives and processes this information.

### Handling Disconnections

- **Server Handling**: The server automatically removes clients from the list view and its internal client list if no communication is received from a client within a 10-second timeframe.
- **Client Handling**: If the client loses connection to the server, it attempts to reconnect every 10 seconds until it successfully reconnects.

### Setup and Usage

1. **Configuration**: Move the file `config.example.yaml` into both folders from where you start the client and server and fill in your details
2. **Server Setup**: Run the server application.
3. **Client Setup**: Run the client application and specify the server's IP address and port.
4. **Data Transmission**: The client sends system information to the server at regular intervals.
5. **Server Processing**: The server receives and processes the information sent by the client.

### Requirements

- .NET Framework or .NET Core installed (depending on the project setup).
