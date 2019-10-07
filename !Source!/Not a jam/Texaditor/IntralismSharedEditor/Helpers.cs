using System;
using System.Collections.Generic;
using System.Text;

namespace IntralismSharedEditor
{
    /// <summary>
    ///  Класс Helpers
    ///  содержит основные переменные
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Пункты, отображаемые в редакторе эвентов
        /// </summary>
        public static List<EditorEventFunctionInfo> eventsMap = new List<EditorEventFunctionInfo>
        {
        new EditorEventFunctionInfo(
            "SpawnObj",
            new List<EditorEventFunctionInfo.EditorEventParametr>()
            {
                new EditorEventFunctionInfo.EditorEventParametr("arc", EditorEventFunctionInfo.EditorEventParametrType.ArcSelector, ""),
                //new EditorEventFunctionInfo.EditorEventParametr("hand", EditorEventFunctionInfo.EditorEventParametrType.Slider, "", "0,2,true,0")
            },
            "Spawn arcs for player. Try to make an interesting generation, synchronized with music"),

        new EditorEventFunctionInfo(
            "Timing",
            new List<EditorEventFunctionInfo.EditorEventParametr>()
            {
                new EditorEventFunctionInfo.EditorEventParametr("frequency", EditorEventFunctionInfo.EditorEventParametrType.InputField, ""),
            },
            "Sets up timing lines"),

        new EditorEventFunctionInfo(
            "Note",
            new List<EditorEventFunctionInfo.EditorEventParametr>()
            {
                new EditorEventFunctionInfo.EditorEventParametr("note", EditorEventFunctionInfo.EditorEventParametrType.InputField, "in seconds", "float,10")
            },
            "For mapping academy"),

        new EditorEventFunctionInfo(
            "SetBGColor",
            new List<EditorEventFunctionInfo.EditorEventParametr>()
            {
                new EditorEventFunctionInfo.EditorEventParametr("red", EditorEventFunctionInfo.EditorEventParametrType.Slider, "", "0,1,false"),
                new EditorEventFunctionInfo.EditorEventParametr("green", EditorEventFunctionInfo.EditorEventParametrType.Slider, "", "0,1,false"),
                new EditorEventFunctionInfo.EditorEventParametr("blue", EditorEventFunctionInfo.EditorEventParametrType.Slider, "", "0,1,false"),
                new EditorEventFunctionInfo.EditorEventParametr("alpha", EditorEventFunctionInfo.EditorEventParametrType.Slider, "", "0,1,false"),
                new EditorEventFunctionInfo.EditorEventParametr("speed", EditorEventFunctionInfo.EditorEventParametrType.InputField, "Lerp speed. Recomended 10", "float,10")
            },
            "Set camera background color"),

        new EditorEventFunctionInfo(
            "SetPlayerDistance",
            new List<EditorEventFunctionInfo.EditorEventParametr>()
            {
                new EditorEventFunctionInfo.EditorEventParametr("distance", EditorEventFunctionInfo.EditorEventParametrType.Slider, "", "4,28,true,0")
            },
            "Set camera (or player) distance. Base player distance - 14"),

        new EditorEventFunctionInfo(
            "ShowTitle",
            new List<EditorEventFunctionInfo.EditorEventParametr>()
            {
                new EditorEventFunctionInfo.EditorEventParametr("title", EditorEventFunctionInfo.EditorEventParametrType.InputField, ""),
                new EditorEventFunctionInfo.EditorEventParametr("duration", EditorEventFunctionInfo.EditorEventParametrType.InputField, "Message duration multiplier. Recomended 1", "float,1")
            },
            "Show text at the center of the screen. Usefull for quick messages like 'Thanks for playing!'"),

        new EditorEventFunctionInfo(
            "ShowSprite",
            new List<EditorEventFunctionInfo.EditorEventParametr>()
            {
                new EditorEventFunctionInfo.EditorEventParametr("resource", EditorEventFunctionInfo.EditorEventParametrType.InputField, ""),
                new EditorEventFunctionInfo.EditorEventParametr("position", EditorEventFunctionInfo.EditorEventParametrType.Slider, "0 - background, 1 - foreground", "0,1,true,0"),
                new EditorEventFunctionInfo.EditorEventParametr("", EditorEventFunctionInfo.EditorEventParametrType.Toggle, "", "keep aspect ratio"),
                new EditorEventFunctionInfo.EditorEventParametr("duration", EditorEventFunctionInfo.EditorEventParametrType.InputField, "in seconds", "float,10")
            },
            "Show image from resources at the center of the screen at foreground or background"),

         new EditorEventFunctionInfo(
            "MapEnd",
            new List<EditorEventFunctionInfo.EditorEventParametr>()
            {

            },
            "Set the end of the map. Use this if you want to cut the long music."),

         
        };

        /// <summary>
        /// все виды арок
        /// </summary>
        public static List<string> patternsMap = new List<string>
        {
            //null
            "[Up]", //0 W
            "[Left]", //1 A
            "[Down]", //2 S
            "[Right]", //3 D
            "[Right-Left]", //4 Q

            //ctrl+
            "[Up-Left]", //5 W
            "[Down-Left]", //6 A
            "[Right-Down]", //7 S
            "[Up-Right]", //8 D
            "[Up-Right-Down-Left]", //9 Q

            //shift+
            "[Up-Right-Left]", //10 W
            "[Up-Down-Left]", //11 A
            "[Right-Down-Left]", //12 S
            "[Up-Right-Down]", //13 D
            "[Up-Down]", //14 Q
            "[PowerUp]", //15 Alt+Z
        };

        /// <summary>
        /// все виды эвентов
        /// </summary>
        public static List<string> eventTypes = new List<string>
        {
            "SpawnObj", // 0 арка 2
            "Timing", // 1 тайминг (игнор игрой) 4
            "Note", // 2 заметка (игнор картой) 1
            "SetBGColor", // 3 2
            "SetPlayerDistance", // 4 1
            "ShowTitle", // 5 2
            "ShowSprite", // 6 3
            "MapEnd" // 7 0
        };

        /// <summary>
        /// тэги воркшопа
        /// </summary>
        public static List<string> tags = new List<string>
        {
            "Alternative",
            "Anime",
            "Blues",
            "Children",
            "Classical",
            "Dance",
            "Electronic",
            "Folk",
            "Hip-hop",
            "Indie",
            "Instrumental",
            "Jazz",
            "Metal",
            "Pop",
            "Rap",
            "Rock",
            "Soundtrack",
            "Other",
        };
        /// <summary>
        /// все  виды арок, но в виде словаря
        /// </summary>
        public static Dictionary<string, string> Arcs = new Dictionary<string, string>
        {
            ["1000"] = "[Up]", //0 W
            ["0010"] = "[Left]", //1 A
            ["0100"] = "[Down]", //2 S
            ["0001"] = "[Right]", //3 D
            ["0011"] = "[Right-Left]", //4 Q
            ["1010"] = "[Up-Left]", //5 W
            ["0110"] = "[Down-Left]", //6 A
            ["0101"] = "[Right-Down]", //7 S
            ["1001"] = "[Up-Right]", //8 D
            ["1111"] = "[Up-Right-Down-Left]", //9 Q
            ["1011"] = "[Up-Right-Left]", //10 W
            ["1110"] = "[Up-Down-Left]", //11 A
            ["0111"] = "[Right-Down-Left]", //12 S
            ["1101"] = "[Up-Right-Down]", //13 D
            ["1100"] = "[Up-Down]", //14 Q
            ["0000"] = "[PowerUp]" //15 Z //NOT USES

        };
    }
}
