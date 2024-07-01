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
  - Operating System
  - Is OS 64 Bit

### How It Works

The server listens for incoming connections from clients. Upon connection, the client sends its system information to the server periodically. The server receives and processes this information.

### Setup and Usage

1. **Server Setup**: Run the server application.
2. **Client Setup**: Run the client application and specify the server's IP address and port.
3. **Data Transmission**: The client sends system information to the server at regular intervals.
4. **Server Processing**: The server receives and processes the information sent by the client.

### Requirements

- .NET Framework or .NET Core installed (depending on the project setup)
