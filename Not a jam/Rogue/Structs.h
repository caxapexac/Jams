#ifndef STRUCTS_DEF
#define STRUCTS_DEF

#include "wchar.h"
#define HEIGHT 256
#define WIDTH 256

enum State
{
    StateIdle = 0,
    StatePlay = 1,
    StateCamera = 2
};

enum Block
{
    BlockEmpty = 0,
    BlockLeaves = 1,
    BlockWood = 2,
    BlockGrass = 3,
    BlockRock = 4,
    BlockCoal = 5,
    BlockBedrock = 6
};

typedef struct Int2
{
    int x;
    int y;
} Int2;

typedef struct Float2
{
    float x;
    float y;
} Float2;

typedef struct Map
{
    int Width;
    int Height;
    enum Block Blocks[WIDTH][HEIGHT];
    wchar_t BotMessage[128];
    wchar_t ScoreMessage[128];
    wchar_t Copyright[128];
    wchar_t ExitMessage[128];
} Map;

typedef struct Player
{
    int Hp;
    Int2 Coords;
} Player;

typedef struct Game
{
    int Score;
    Int2 ScreenPos;
    Int2 ScreenSize;
    Map Map;
    Player Player;
    enum State State;
    int Octaves;
    float Frequency;
} Game;



#endif
