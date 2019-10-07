#ifndef ROGUE_DEF
#define ROGUE_DEF

#define CAPACITY 8

#include <stdio.h>
#include <stdlib.h>
#include <locale.h>
#include <time.h>
//#include <unistd.h>

#include "Renderer.h"
#include "Structs.h"
#include "ColorDefinitions.h"5
#include "LinuxSpecial.h"
#include "MacroHell.h"
#include "PerlinNoise.h"

//************************
//Rogue - perlin roguelike
//************************
//Globals:

static Game GameState;

static float Mills = 1.0 / 0.01 * 100.0;

static Int2 Direction;

static int Respect = 0;

static clock_t StartTime;

int main()
{
    StartTime = clock();
    setlocale(LC_ALL, "");
    system("setterm -cursor off");
    GameState.Frequency = 0.015;
    GameState.Octaves = 5;
    GameState.State = StatePlay;
    GameState.Score = 0;
    GameState.ScreenSize = GetConsoleSize();
    GameState.ScreenPos.x = 0;
    GameState.ScreenPos.y = 0;
    GameState.Map.Width = WIDTH;
    GameState.Map.Height = HEIGHT;
    wcscpy(GameState.Map.BotMessage, L"╗ WELCOME! Btw this one is mush better than Fallout76 ;) ╔");
    wcscpy(GameState.Map.Copyright, L"╗ ©2019 ╔");
    wcscpy(GameState.Map.ExitMessage, L"╝ Press F to pay respect, Press ESC to exit ╚");
    GameState.Player.Coords.x = GameState.ScreenPos.x + GameState.ScreenSize.x / 2;
    GameState.Player.Coords.y = GameState.ScreenPos.y + GameState.ScreenSize.y / 2;
    Direction.x = 0;
    Direction.y = 0;
    srand(clock());
    seed(rand());


    WSetCursor(GameState.Player.Coords.x - GameState.ScreenPos.x + 1, GameState.Player.Coords.y - GameState.ScreenPos.y);
    changemode(1);
    char t = 0;
    char t2 = 0;
    do
    {
        GameState.Score += rand() % 10;
        swprintf(GameState.Map.ScoreMessage, 128, L"╝ Your fucking score = %d ╚", GameState.Score);
        if (kbhit())
        {
            t = lgetch();
            switch (GameState.State)
            {
            case StateIdle:
                switch (t)
                {
                case '\\':
                    GameState.State = StatePlay;
                    break;
                default:
                    break;
                }
                break;
            case StatePlay:
                switch (t)
                {
                case 'w':
                case 'W':
                    GameState.Player.Coords.y--;
                    Direction.y = CLIP(Direction.y - 1, -1, 1);
                    if (GameState.Player.Coords.y <= GameState.ScreenPos.y + GameState.ScreenSize.y / 4)
                    {
                        GameState.ScreenPos.y--;
                    }
                    swprintf(GameState.Map.BotMessage, 128, L"╗ You travel north ╔");
                    break;
                case 'a':
                case 'A':
                    GameState.Player.Coords.x--;
                    Direction.x = CLIP(Direction.x - 1, -1, 1);
                    if (GameState.Player.Coords.x <= GameState.ScreenPos.x + GameState.ScreenSize.x / 4)
                    {
                        GameState.ScreenPos.x--;
                    }
                    swprintf(GameState.Map.BotMessage, 128, L"╗ You travel left ╔");
                    break;
                case 's':
                case 'S':
                    GameState.Player.Coords.y++;
                    Direction.y = CLIP(Direction.y + 1, -1, 1);
                    if (GameState.Player.Coords.y >= GameState.ScreenPos.y + GameState.ScreenSize.y / 4 * 3)
                    {
                        GameState.ScreenPos.y++;
                    }
                    swprintf(GameState.Map.BotMessage, 128, L"╗ You travel nahui ╔");
                    break;
                case 'd':
                case 'D':
                    GameState.Player.Coords.x++;
                    Direction.x = CLIP(Direction.x + 1, -1, 1);
                    if (GameState.Player.Coords.x >= GameState.ScreenPos.x + GameState.ScreenSize.x / 4 * 3)
                    {
                        GameState.ScreenPos.x++;
                    }
                    swprintf(GameState.Map.BotMessage, 128, L"╗ You travel east ╔");
                    break;
                case 'q':
                case 'Q':
                    GameState.Octaves = CLIP(GameState.Octaves - 1, 1, 20);
                    swprintf(GameState.Map.BotMessage, 128, L"╗ Octaves = %d ╔", GameState.Octaves);
                    break;
                case 'e':
                case 'E':
                    GameState.Octaves = CLIP(GameState.Octaves + 1, 1, 20);
                    swprintf(GameState.Map.BotMessage, 128, L"╗ Octaves = %d ╔", GameState.Octaves);
                    break;
                case 'z':
                case 'Z':
                    GameState.Frequency = CLIP(GameState.Frequency - 0.001f, 0.001f, 10);
                    swprintf(GameState.Map.BotMessage, 128, L"╗ Frequency = %f ╔", GameState.Frequency);
                    break;
                case 'x':
                case 'X':
                    GameState.Frequency = CLIP(GameState.Frequency + 0.001f, 0.001f, 10);
                    swprintf(GameState.Map.BotMessage, 128, L"╗ Frequency = %f ╔", GameState.Frequency);
                    break;
                case 'f':
                case 'F':
                    if (Respect) {
                        Respect = 0;
                    }
                    else
                    {
                        Respect = 1;
                    }
                    break;
                case 'c':
                case 'C':
                    seed(rand());
                    break;
                case '\n':
                case 27: //escape
                    //exit
                    //wcscpy(GameState.Map.BotMessage, L"press escape twice to exit");
                    break;
                default:
                    //wprintf(L"%c=%d %c=%d\n", t, t, t2, t2);
                    break;
                }
            case StateCamera:
                //
                GameState.State = StatePlay;
                //
                break;
            }
        }
        else
        {
            GameState.Player.Coords.x += Direction.x;
            GameState.Player.Coords.y += Direction.y;
            while (GameState.Player.Coords.y <= GameState.ScreenPos.y + GameState.ScreenSize.y / 4)
            {
                GameState.ScreenPos.y--;
            }
            while (GameState.Player.Coords.x <= GameState.ScreenPos.x + GameState.ScreenSize.x / 4)
            {
                GameState.ScreenPos.x--;
            }
            while (GameState.Player.Coords.y >= GameState.ScreenPos.y + GameState.ScreenSize.y / 4 * 3)
            {
                GameState.ScreenPos.y++;
            }
            while (GameState.Player.Coords.x >= GameState.ScreenPos.x + GameState.ScreenSize.x / 4 * 3)
            {
                GameState.ScreenPos.x++;
            }
        }
    if (Respect)
    {
        RenderPika(&GameState);
    }
    else
    {
        RenderAll(&GameState);
    }
        
        usleep(Mills);
    } while (t != 27);
    changemode(0);
    WSetCursor(1, GameState.ScreenSize.y);
    system("setterm -cursor on");
    printf("\n");
    return 0;
}

#endif
