#include "Client.h"

SOCKET Client::getClientSocket() {
	return clientSocket;
}

Client::Client(SOCKET clientSocket) {
	this->clientSocket = clientSocket;
}

bool Client::setClientID(std::string client, std::vector<Client*> connections) {
	mClient->lock();
	for (int i = 0; i < connections.size(); i++) {
		if (client == connections[i]->getClientID()) {
			mClient->unlock();
			return false;
		}
	}
	mClient->unlock();
	this->clientID = client;
	return true;
}
std::string Client::getClientID() {
	return clientID;
}

std::mutex* Client::getMutex() {
	return this->mClient;
}
void Client::setMutex(std::mutex* mClient) {
	this->mClient = mClient;
}