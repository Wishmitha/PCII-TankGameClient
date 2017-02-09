**Team Members:**

Wishmitha Mendis

Adeesha Jaysooriya

**Description :**

A GUI client developed for the Tank Game Server in CS 2212. (Programming Challenge II) The client is capable is capable of passing messages to the server and display data sent by the server such as map information, player positions details in the local GUI. And it also consists with an algorithm to automatically play the ame against the joined player to the server.

**Implementation :**

The client is initially connected to the server by sending #JOIN messeage. Then the algorithm automatically generates commands to control the tank according to the sent data from the server. These commands are sent as messages such as #LEFT #RIGHT #SHOOT to the server and it is updated. And the client will be also updated according to the server.

**Development :**

The client was developed using Unity. Unity 2D package was used to render the GUI and C# language was used to listen to the server, perform updates in the client and run the algorithm.
