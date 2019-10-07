#include <stdio.h>
// (c) Омар
// \033 == \x1b

//***********
// LINUX ONLY
//***********

#define ClearConsole() printf("\033[2J") // или "\033[H\033[J"
#define SetCursor(x,y) printf("\033[%d;%dH", (y), (x))
#define MoveCursorUp(x) printf("\033[%dA", (x));
#define MoveCursorDown(x) printf("\033[%dB", (x));
#define MoveCursorRight(x) printf("\033[%dC", (x));
#define MoveCursorLeft(x) printf("\033[%dD", (x));

#define WClearConsole() wprintf(L"\033[2J") // или "\033[H\033[J"
#define WSetCursor(x,y) wprintf(L"\033[%d;%dH", (y), (x))
#define WMoveCursorUp(x) wprintf(L"\033[%dA", (x));
#define WMoveCursorDown(x) wprintf(L"\033[%dB", (x));
#define WMoveCursorRight(x) wprintf(L"\033[%dC", (x));
#define WMoveCursorLeft(x) wprintf(L"\033[%dD", (x));

#define NONE "\033[0m" //все атрибуты по умолчанию
#define TBOLD "\033[1m" //жирный текст (интенсивный цвет)
#define T "\033[2m" //полу яркий текст (тёмно-серый, независимо от цвета)
#define M3 "\033[3m" //наклонный текст
#define M4 "\033[4m" //подчеркивание текста
#define M5 "\033[5m" //мигающий текст
#define M6 "\033[6m" //невидимый текст
#define M7 "\033[7m" //реверсия (текст приобретает цвет фона, а фон - текста)
#define M8 "\033[8m" //
#define M9 "\033[9m" //зачеркнутый текст

#define R1 "\033[22m" // установить нормальную интенсивность
#define R2 "\033[24m" // отменить подчеркивание
#define R3 "\033[25m" // отменить мигание
#define R4 "\033[27m" // отменить реверсию

//цвет текста
#define FDEFAULT "\033[39m"

#define FBLACK "\033[30m"
#define FDRED "\033[31m"
#define FDGREEN "\033[32m"
#define FDYELLOW "\033[33m"
#define FDBLUE "\033[34m"
#define FDMAGNETA "\033[35m"
#define FDCYAN "\033[36m"
#define FGRAY "\033[37m"

#define FDGRAY "\033[90m"
#define FRED "\033[91m"
#define FGREEN "\033[92m"
#define FYELLOW "\033[93m"
#define FBLUE "\033[94m"
#define FMAGNETA "\033[95m"
#define FCYAN "\033[96m"
#define FWHITE "\033[97m"

//цвет фона
#define BDEFAULT "49m"

#define BBLACK "40m"
#define BDRED "41m"
#define BDGREEN "42m"
#define BDYELLOW "43m"
#define BDBLUE "44m"
#define BDMAGNETA "45m"
#define BDCYAN "46m"
#define BGRAY "47m"

#define BDGRAY "100m"
#define BRED "101m"
#define BGREEN "102m"
#define BYELLOW "103m"
#define BBLUE "104m"
#define BMAGNETA "105m"
#define BCYAN "106m"
#define WHITE "107m"
