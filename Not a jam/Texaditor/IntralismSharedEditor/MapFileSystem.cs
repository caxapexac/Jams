using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IntralismSharedEditor
{
    /// <summary>
    /// Класс, который может создать папку карты и всё чётко оформить
    /// </summary>
    public class MapFileSystem
    {

        /// <summary>
        /// Стандартная директория для карт
        /// </summary>
        public string DefDir;

        /// <summary>
        /// Директория карты
        /// </summary>
        public string MapDirName;

        /// <summary>
        /// Название песни (по умолчанию music.ogg)
        /// </summary>
        public string MusicName;

        /// <summary>
        /// Название конфига (по умолчанию map.data)
        /// </summary>
        public string ConfigName;

        /// <summary>
        /// Название иконки (по умолчанию icon.jpg)
        /// </summary>
        public string IconName;

        /// <summary>
        /// возвращает путь к музыке
        /// </summary>
        public string MusicPath{ get { return DefDir + "\\" + MapDirName + "\\" + MusicName; } }

        /// <summary>
        /// возвращает путь к конфигу
        /// </summary>
        public string ConfigPath { get { return DefDir + "\\" + MapDirName + "\\" + ConfigName; } }

        /// <summary>
        /// возвращает путь к конфигу
        /// </summary>
        public string IconPath { get { return DefDir + "\\" + MapDirName + "\\" + IconName; } }

        /// <summary>
        /// возвращает директорию ресурсов
        /// </summary>
        public string ResDir { get { return DefDir + "\\" + MapDirName + "\\" + "resources"; } }

        /// <summary>
        /// возвращает директорию бэкапов
        /// </summary>
        public string BackDir { get { return DefDir + "\\" + MapDirName + "\\" + "backup"; } }

        /// <summary>
        /// возвращает директорию карты
        /// </summary>
        public string MapDir
        {
            get { return DefDir + "\\" + MapDirName; }
            set
            {
                DefDir = value.Substring(0, value.LastIndexOf("\\"));
                MapDirName = value.Substring(value.LastIndexOf("\\") + 1);
            }
        }

        /// <summary>
        /// пустой конструктор
        /// </summary>
        public MapFileSystem()
        {
            DefDir = ConfigReg.DirDefault;
            MapDirName = "Map" + DateTime.Now.ToLongTimeString().Replace(":", "") + CustomEditor.rand.Next(1000);
            MusicName = "music.ogg";
            ConfigName = "map.data";
            IconName = "icon.jpg";
        }

        /// <summary>
        /// Создать новую директорию
        /// </summary>
        /// <param name="path">Путь к музыке</param>
        public MapFileSystem(string path) : this()
        {
            Directory.CreateDirectory(MapDir);
            Directory.CreateDirectory(ResDir);
            Directory.CreateDirectory(BackDir);
            File.Copy(path, MusicPath, true);
        }

        /// <summary>
        /// Создать директорию (если .txt) | Загрузить директорию (если .data)
        /// </summary>
        /// <param name="path">Путь к музыке</param>
        /// <param name="cpath">Путь к конфигу</param>
        /// <param name="list">Список ресурсов</param>
        /// <param name="icon">Имя иконки</param>
        public MapFileSystem(string path, string cpath, List<MapResource> list, string icon = null) : this()
        {
            if (cpath.EndsWith(".txt"))
            {
                string oldPath = cpath.Substring(0, cpath.LastIndexOf("\\"));
                MusicName = path;
                Directory.CreateDirectory(MapDir);
                Directory.CreateDirectory(ResDir);
                Directory.CreateDirectory(BackDir);
                File.Copy(oldPath + "\\" + path, MusicPath, true);
                File.Copy(cpath, ConfigPath, true);
                if(list != null)
                {
                    foreach (MapResource res in list)
                    {
                        //MessageBox.Show(res.name + " " + res.path + " " + oldPath);
                        if (File.Exists(oldPath + "\\" + res.path))
                        {
                            File.Copy(oldPath + "\\" + res.path, ResDir + "\\" + res.path, true);
                        }
                        else
                        {
                            File.Create(ResDir + "\\" + res.path);
                            MessageBox.Show("DAMN! I CANT FIND " + oldPath + "\\" + res.path);
                        }
                    }
                }
                if (icon != null)
                {
                    IconName = icon;
                    if (File.Exists(oldPath + "\\" + icon)) File.Copy(oldPath + "\\" + icon, IconPath); else MessageBox.Show("ICON DOESNT EXIST");
                }
            }
            else if (cpath.EndsWith(".data"))
            {
                MapDir = cpath.Substring(0, cpath.LastIndexOf("\\"));
                MusicName = path;
            }
            
        }
    }
}