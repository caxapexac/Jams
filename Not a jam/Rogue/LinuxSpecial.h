#include <sys/ioctl.h>
#include <stdio.h>
#include <unistd.h>

#include "Structs.h"

Int2 GetConsoleSize(void);
char lgetch();
char lgetche();
void changemode(int);
int kbhit();
