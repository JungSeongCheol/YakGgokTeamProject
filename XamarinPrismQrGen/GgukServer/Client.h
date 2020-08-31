#pragma once
#include <Windows.h>
#include <string>
#include <vector>
#include <mutex>

class Client {
private:
	SOCKET clientSocket;
	std::string clientID;
	std::mutex* mClient;
public:
	Client(SOCKET clientSocket);
	SOCKET getClientSocket();
	bool setClientID(std::string client, std::vector<Client*> connections);
	std::string getClientID();

	std::mutex* getMutex();
	void setMutex(std::mutex* mClient);
};