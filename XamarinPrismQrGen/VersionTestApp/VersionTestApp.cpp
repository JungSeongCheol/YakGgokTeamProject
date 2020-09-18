#include <stdio.h>
#include <iostream>
#include <crtdbg.h>

#if _DEBUG
#define new new(_NORMAL_BLOCK, __FILE__, __LINE__)
#define malloc(s) _malloc_dbg(s, _NORMAL_BLOCK, __FILE__, __LINE__)
#endif

using namespace std;

int main()
{
	cout << _MSC_VER << endl;
	_CrtDumpMemoryLeaks();
	return 0;
}