#include <stdio.h>
#include <wchar.h>
#include <wctype.h>

#include "Structs.h"

void Flush();
void RenderUi(Game *game, wchar_t *text);
void RenderAll(Game *game);
void RenderPika(Game *game);
void RenderGround(Game *game);
void RenderPlayer(Game *game);
