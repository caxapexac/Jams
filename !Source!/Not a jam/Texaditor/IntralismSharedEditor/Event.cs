using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace IntralismSharedEditor
{
    /// <summary>
    /// новый класс эвента
    /// </summary>
    public class Event
    {
        private Border border = new Border()
            {
                Margin = new Thickness(3000, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
            };
        private double time;
        private bool select = false;
        //=========================================================
        /// <summary>
        /// храниит значение передвигается ли эвент
        /// </summary>
        public bool IsMoving = false;
        /// <summary>
        /// для эвент конфига
        /// </summary>
        public bool NeedUpdate = false;
        /// <summary>
        /// для эвент конфига
        /// </summary>
        public bool NeedRemove = false;
        /// <summary>
        /// для эвент конфига
        /// </summary>
        public bool UI;
        /// <summary>
        /// объект эвента для MapData
        /// </summary>
        public List<MapEvent> mapEventList = new List<MapEvent>();
        //=========================================================
        /// <summary>
        /// возвращает/задаёт время эвента
        /// </summary>
        public double Time
        {
            get { return time; }
            set
            {
                time = value;
                border.Tag = time;
                foreach (MapEvent mevent in mapEventList)
                {
                    mevent.time = time;
                }
            }
        }
        /// <summary>
        /// возвращает/задаёт цвет эвента
        /// </summary>
        public SolidColorBrush BorderColor
        {
            get
            {
                return ((SolidColorBrush)((Border)border.Child)?.OpacityMask)?? new SolidColorBrush(Colors.Black);
            }
            set
            {
                ((Border)border.Child).OpacityMask = value;
            }
        }
        /// <summary>
        /// возвращает/задаёт выделен ли эвент
        /// </summary>
        public bool IsSelected
        {
            get { return select; }
            set { select = value; }
        }
        /// <summary>
        /// возвращает/задаёт свойство Margin.Left
        /// </summary>
        public double LeftMargin
        {
            get
            {
                return border?.Margin.Left??0;
            }
            set
            {
                if (border != null)
                {
                    border.Margin = new Thickness(value, border.Margin.Top, border.Margin.Right, border.Margin.Bottom);
                }
            }
        }
        //=========================================================
        /// <summary>
        /// возвращает свойство ActualWidth
        /// </summary>
        public double BorderWidth
        {
            get { return ((Border)border.Child)?.ActualWidth??50; }
        }
        /// <summary>
        /// возвращает тип эвента SpawnObj/Timing/Storyboard
        /// </summary>
        public string Type
        {
            get
            {
                if (mapEventList.Count == 0) return "UNKNOWN EVENT TYPE CUZ MAPEVENTLIST.COUNT = 0";
                switch (mapEventList[0].data[0])
                {
                    case "SpawnObj": return "Gameplay";
                    case "Timing": return "Mapping";
                    case "Note": return "Mapping";
                    case "SetBGColor": return "Storyboard";
                    case "SetPlayerDistance": return "Storyboard";
                    case "ShowTitle": return "Storyboard";
                    case "ShowSprite": return "Storyboard";
                    case "MapEnd": return "Storyboard";
                    default: MessageBox.Show("UNKNOWN EVENT TYPE " + mapEventList[0].data[0]); return mapEventList[0].data[0];
                }
            }
        }
        /// <summary>
        /// возвращает количество эвентов
        /// </summary>
        public int EventCount
        {
            get { return mapEventList.Count; }
        }
        /// <summary>
        /// возвращает ссылку на border эвента
        /// </summary>
        public Border GetBorder
        {
            get { return border; }
        }
        //=========================================================
        /// <summary>
        /// попытаться добавить мапэвент в Event 
        /// </summary>
        /// <param name="mevent">эвент</param>
        public bool TryAddEvent(MapEvent mevent)
        {
            if (mevent.time == Time)
            {
                
                string type = "";
                switch (mevent.data[0])
                {
                    case "SpawnObj": type = "Gameplay"; break;
                    case "Timing": type = "Mapping"; break;
                    case "Note": type = "Mapping"; break;
                    case "SetBGColor": type = "Storyboard"; break;
                    case "SetPlayerDistance": type = "Storyboard"; break;
                    case "ShowTitle": type = "Storyboard"; break;
                    case "ShowSprite": type = "Storyboard"; break;
                    case "MapEnd": type = "Storyboard"; break;
                    default: return false;
                }
                if (type == Type)
                {
                    MapEvent err = mapEventList.Find(i => i.data[0] == mevent.data[0]);
                    if (err != null)
                    {
                        if (mevent.data[0] == "SpawnObj")
                        {
                            string oldArc = err.data[1];
                            string newArc = mevent.data[1];
                            string key = "";
                            if (oldArc.Contains("Up") | newArc.Contains("Up")) key += "1"; else key += "0";
                            if (oldArc.Contains("Down") | newArc.Contains("Down")) key += "1"; else key += "0";
                            if (oldArc.Contains("Left") | newArc.Contains("Left")) key += "1"; else key += "0";
                            if (oldArc.Contains("Right") | newArc.Contains("Right")) key += "1"; else key += "0";
                            mevent.data[1] = Helpers.Arcs[key];
                        }
                        mapEventList.Remove(err);
                    }
                    mapEventList.Add(mevent);
                    SetDefaultData(mevent.data);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// попытаться добавить мапэвент в Event 
        /// </summary>
        /// <param name="data">тип</param>
        public void AddEvent(string data)
        {
            string type = "";
            switch (data)
            {
                case "SpawnObj": type = "Gameplay"; break;
                case "Timing": type = "Mapping"; break;
                case "Note": type = "Mapping"; break;
                case "SetBGColor": type = "Storyboard"; break;
                case "SetPlayerDistance": type = "Storyboard"; break;
                case "ShowTitle": type = "Storyboard"; break;
                case "ShowSprite": type = "Storyboard"; break;
                case "MapEnd": type = "Storyboard"; break;
                default: break;
            }
            if (type == Type)
            {
                MapEvent err = mapEventList.Find(i => i.data[0] == data);
                if (err != null) MessageBox.Show("ERROR 0x000021 INFO:" + err.data[0]); mapEventList.Remove(err); 
                MapEvent one = new MapEvent(Time, new List<string>() { data });
                mapEventList.Add(one);
                SetDefaultData(one.data);
            }
            else MessageBox.Show("ERROR 0x000022 INFO:" + data);

        }
        /// <summary>
        /// попытаться удалить мапэвент из Event 
        /// </summary>
        /// <param name="type">тип</param>
        public bool TryRemoveEvent(string type)
        {
            MapEvent err = mapEventList.Find(i => i.data[0] == type);
            if (err != null)
            {
                mapEventList.Remove(err);
                if (mapEventList.Count == 0) NeedRemove = true;
                return true;
            }
            return false;
        }
        /// <summary>
        /// попробовать изменить время эвента
        /// </summary>
        /// <param name="t">время</param>
        /// <returns></returns>
        public bool TrySetTime(string t)
        {
            if (double.TryParse(t, out double d))
            {
                Time = d;
                NeedUpdate = true;
                return true;
            }
            return false;
        }
        /// <summary>
        /// возвращает данные эвента под определенным индексом
        /// </summary>
        /// <param name="type">тип</param>
        /// <param name="index">индекс</param>
        /// <returns></returns>
        public string GetData (int index, string type = null)
        {
            if(type == null)
            {
                if (mapEventList.Count == 1)
                {
                    if (mapEventList[0] != null && index < mapEventList[0].data.Count)
                    {
                        return mapEventList[0].data[index];
                    }
                }
                return "ERROR FINDING DATA ON " + index;
            }
            else
            {
                MapEvent mevent = mapEventList.Find(i => i.data[0] == type);
                if (mevent != null && index < mevent.data.Count) return mevent.data[index]; else return "ERROR FINDING DATA " + type + " ON " + index;
            }
        }
        /// <summary>
        /// записать данные под определенным индексом в эвент
        /// </summary>
        /// <param name="type">тип</param>
        /// <param name="index">индекс</param>
        /// <param name="data">данные</param>
        /// <returns></returns>
        //=========================================================
        public void SetData(int index, string data, string type = null)
        {
            MapEvent mevent;
            if (type == null)
            {
                if (mapEventList.Count != 1) MessageBox.Show("ERROR FINDING EVENT");
                mevent = mapEventList[0];
            }
            else
            {
                mevent = mapEventList.Find(i => i.data[0] == type);
            }
            if (mevent != null && index > 0)
            {
                switch (type)
                {
                    case "SpawnObj":
                        while (mevent.data.Count < 3) mevent.data.Add("");
                        switch (index)
                        {
                            case 1:
                                if (Helpers.patternsMap.Contains(data)) mevent.data[1] = data;
                                break;
                            case 2:
                                if (data.Contains(" ") == false && int.TryParse(data, out int d) && d >= 0 && d <= 2) mevent.data[2] = data;
                                break;
                            default:
                                MessageBox.Show("ERROR FINDING INDEX " + index + " ON THE EVENT " + type);
                                break;
                        }
                        break;
                    case "Timing":
                        while (mevent.data.Count < 5) mevent.data.Add("");
                        switch (index)
                        {
                            case 1:
                                data = data.Replace(".", ",");
                                if (data.Contains(" ") == false && double.TryParse(data, out double buf1)) mevent.data[1] = data;
                                break;
                            case 2:
                                data = data.Replace(".", ",");
                                if (data.Contains(" ") == false && double.TryParse(data, out double buf2)) mevent.data[2] = data;
                                break;
                            case 3:
                                if (data.Contains(" ") == false && int.TryParse(data, out int buf3)) mevent.data[3] = data;
                                break;
                            case 4:
                                if (data.Contains(" ") == false && int.TryParse(data, out int buf4)) mevent.data[4] = data;
                                break;
                            default:
                                MessageBox.Show("ERROR FINDING INDEX " + index + " ON THE EVENT " + type);
                                break;
                        }
                        break;
                    case "Note":
                        while (mevent.data.Count < 2) mevent.data.Add("");
                        switch (index)
                        {
                            case 1:
                                mevent.data[1] = data;
                                break;
                            default:
                                MessageBox.Show("ERROR FINDING INDEX " + index + " ON THE EVENT " + type);
                                break;
                        }
                        break;
                    case "SetBGColor":
                        while (mevent.data.Count < 5) mevent.data.Add("");
                        switch (index)
                        {
                            case 1:
                                data = data.Replace(".", ",");
                                if (data.Contains(" ") == false && double.TryParse(data, out double res1) && res1 >= 0 && res1 <= 1) mevent.data[1] = data;
                                break;
                            case 2:
                                data = data.Replace(".", ",");
                                if (data.Contains(" ") == false && double.TryParse(data, out double res2) && res2 >= 0 && res2 <= 1) mevent.data[2] = data;
                                break;
                            case 3:
                                data = data.Replace(".", ",");
                                if (data.Contains(" ") == false && double.TryParse(data,out double res3) && res3 >= 0 && res3 <= 1) mevent.data[3] = data;
                                break;
                            case 4:
                                data = data.Replace(".", ",");
                                if (data.Contains(" ") == false && double.TryParse(data, out double buf4) && buf4 > 0 && buf4 <= 100) mevent.data[4] = data;
                                break;
                            default:
                                MessageBox.Show("ERROR FINDING INDEX " + index + " ON THE EVENT " + type);
                                break;
                        }
                        break;
                    case "SetPlayerDistance":
                        while (mevent.data.Count < 2) mevent.data.Add("");
                        switch (index)
                        {
                            case 1:
                                if (data.Contains(" ") == false && int.TryParse(data, out int d) && d > 0 && d <= 30) mevent.data[1] = data;
                                break;
                            //case 2:
                            //if (double.TryParse(data, out double d) && d > 0 && d <= 30) mevent.data[2] = data;
                            //break;
                            default:
                                MessageBox.Show("ERROR FINDING INDEX " + index + " ON THE EVENT " + type);
                                break;
                        }
                        break;
                    case "ShowTitle":
                        while (mevent.data.Count < 3) mevent.data.Add("");
                        switch (index)
                        {
                            case 1:
                                mevent.data[1] = data;
                                break;
                            case 2:
                                data = data.Replace(".", ",");
                                if (data.Contains(" ") == false && double.TryParse(data, out double d) && d > 0) mevent.data[2] = data;
                                break;
                            //case 3:
                            //if (double.TryParse(data, out double d) && d > 0 && d <= 30) mevent.data[3] = data;
                            //break;
                            default:
                                MessageBox.Show("ERROR FINDING INDEX " + index + " ON THE EVENT " + type);
                                break;
                        }
                        break;
                    case "ShowSprite":
                        while (mevent.data.Count < 5) mevent.data.Add("");
                        switch (index)
                        {
                            case 1:
                                mevent.data[1] = data;
                                break;
                            case 2:
                                if (data.Contains(" ") == false && int.TryParse(data, out int buf2) && buf2 >= 1 && buf2 <= 2) mevent.data[2] = data;
                                break;
                            case 3:
                                if (data.Contains(" ") == false && bool.TryParse(data, out bool b)) mevent.data[3] = data;
                                break;
                            case 4:
                                data = data.Replace(".", ",");
                                if (data.Contains(" ") == false && double.TryParse(data, out double d) && d > 0) mevent.data[4] = data;
                                break;
                            //case 4:
                            //if (double.TryParse(data, out double d) && d > 0 && d <= 30) mevent.data[4] = data;
                            //break;
                            default:
                                MessageBox.Show("ERROR FINDING INDEX " + index + " ON THE EVENT " + type);
                                break;
                        }
                        break;
                    case "MapEnd":
                        while (mevent.data.Count < 1) mevent.data.Add("");
                        switch (index)
                        {
                            //case 1:
                            //if (int.TryParse(data, out int d) && d > 0 && d <= 30) mevent.data[1] = data;
                            //break;
                            default:
                                MessageBox.Show("ERROR FINDING INDEX " + index + " ON THE EVENT " + type);
                                break;
                        }
                        break;
                    default:
                        MessageBox.Show("UNKNOWN EVENT TYPE " + type);
                        break;
                }
                //NeedUpdate = true;
            }
            else
            {
                MessageBox.Show("ERROR SETTING EVENT " + type + " INTO " + index + " : " + data);
            }
        }
        /// <summary>
        /// устанавливает стандартные значения
        /// </summary>
        /// <param name="data">новое значение data[0]</param>
        public void SetDefaultData(List<string> data)
        {
            List<string> bufData = new List<string>(data);
            switch (data[0])
            {
                case "SpawnObj":
                    SetData( 1, "Up", data[0]); //arc type
                    SetData( 2, "0", data[0]); //hand
                    break;
                case "Timing":
                    SetData( 1, "0", data[0]); //frequency
                    SetData( 2, "0", data[0]); //first tick time
                    SetData( 3, "4", data[0]); //width
                    SetData( 4, "1", data[0]); //tick number
                    break;
                case "Note":
                    SetData( 1, "NLOLICON WHERE ARE YOU?!     Try to find all the easter eggs.", data[0]); //text
                    break;
                case "SetBGColor":
                    SetData( 1, "0", data[0]); //color
                    SetData( 2, "0", data[0]); //color
                    SetData( 3, "0", data[0]); //color
                    SetData( 4, "5", data[0]); //speed
                    break;
                case "SetPlayerDistance":
                    SetData( 1, "14", data[0]); //distance
                    //SetData(data[0], 2, "1"); //speed
                    break;
                case "ShowTitle":
                    SetData( 1, "Mapped in the Texaditor",data[0]); //text
                    SetData( 2, "3",data[0]); //length
                    //SetData(data[0], 3, "1"); //speed
                    break;
                case "ShowSprite":
                    SetData( 1, "",data[0]); //name
                    SetData( 2, "1",data[0]); //background / foreground
                    SetData( 3, "true", data[0]); //length
                    SetData( 4, "5",data[0]); //length
                    //SetData(data[0], 4, "1"); //speed
                    break;
                case "MapEnd":
                    //SetData(data[0], 1, "END"); 
                    break;
                default:
                    MessageBox.Show("UNKNOWN EVENT TYPE " + data[0]);
                    break;
            }
            for (int i = 1; i < bufData.Count; i++) SetData(i, bufData[i], bufData[0]);
        }
        /// <summary>
        /// переключает выделение эвента
        /// </summary>
        public void ChangeSelect()
        {
            if (IsSelected) IsSelected = false; else IsSelected = true;
        }
        //=========================================================
        /// <summary>
        /// создаёт эвенты при копировании
        /// </summary>
        public Event(Event one)
        {
            Time = one.Time;
            IsSelected = one.IsSelected;
            foreach (MapEvent mevent in one.mapEventList)
            {
                MapEvent mew = new MapEvent(mevent);
                mapEventList.Add(mew);
                SetDefaultData(mew.data);
            }
        }
        /// <summary>
        /// создаёт эвенты при загрузке карты
        /// </summary>
        /// <param name="mevent">эвент</param>
        public Event(MapEvent mevent)
        {
            Time = mevent.time;
            mapEventList.Add(mevent);
            SetDefaultData(mapEventList[0].data);
        }
        /// <summary>
        /// cоздаёт эвент
        /// </summary>
        /// <param name="time">время</param>
        /// <param name="data">тип + данные</param>
        public Event(double time, List<string> data)
        {
            this.Time = time;
            mapEventList.Add(new MapEvent(time,data));
            SetDefaultData(data);
        }
    }
}
