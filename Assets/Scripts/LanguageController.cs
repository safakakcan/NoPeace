using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageController : MonoBehaviour
{
    public UnityEngine.UI.Text[] texts = new UnityEngine.UI.Text[0];

    public SystemLanguage language = SystemLanguage.Unknown;

    public Dictionary<string, string> EN = new Dictionary<string, string>()
    {
        {"gamespeed", "Game Speed"},
        {"continue", "Continue"},
        {"newgame", "New Game"},
        {"savegame", "Save Game"},
        {"loadgame", "Load Game"},
        {"quit", "Quit"},
        {"gamemenu", "Game Menu"},
        {"selectscenario", "Select a scenario"},
        {"selectteam", "Select your team"},
        {"treasure", "Treasure: {0}"},
        {"population", "Population: {0}"},
        {"militarydegree", "Military Degree: +{0}%"},
        {"populationgrowth", "Population Growth: +{0}%"},
        {"taxrate", "Tax Rate: +{0}%"},
        {"raisetroop", "Raise Troop"},
        {"growpopulation", "Grow Population"},
        {"improveeconomy", "Improve Economy"},
        {"cost", "{0} GOLD, {1} POPULATION"},
        {"status", "Status: {0}"},
        {"wait", "Wait"},
        {"move", "Move"},
        {"attack", "Attack"},
        {"defend", "Defend"},
        {"split", "Split"},
        {"retreat", "Retreat"},
        {"gamesaved", "Game saved successfully."},
        {"nosave", "No save found."},
        {"calltroops", "Call Troops"},
        {"excludefighting", "Exclude Fightings"},
        {"onlyhighmorale", "Only High Morale"},
        {"call", "Call"},
        {"directedunits", "{0} troops were directed to {1}."},
        {"Europe 1500", "Europe 1500"},
        {"Warring States Of China","Warring States Of China"},
        {"Ottoman Empire", "Ottoman Empire"},
        {"Hungary", "Hungary"},
        {"Holy Roman Empire", "Holy Roman Empire"},
        {"France", "France"},
        {"Britain", "Britain"},
        {"Castile", "Castile"},
        {"Venice", "Venice"},
        {"Poland-Lithuania", "Poland-Lithuania"},
        {"Naples", "Naples"},
        {"Kalmar Union", "Kalmar Union"},
        {"Crimea", "Crimea"},
        {"Portugal", "Portugal"},
        {"Russia", "Russia"},
        {"Armenia", "Armenia"},
        {"Azerbaijan", "Azerbaijan"},
        {"Caucasus Conflict", "Caucasus Conflict"},
        {"Divided Island", "Divided Island"},
        {"Syrian Civil War", "Syrian Civil War"},
        {"Goverment Forces", "Goverment Forces"},
        {"Rebel Forces", "Rebel Forces"},
        {"Kurdish Forces", "Kurdish Forces"},
        {"ISIS", "ISIS"},
        {"won", "{0} won!"},
        {"thanks", "Thanks for Support!"}
    };

    public Dictionary<string, string> TR = new Dictionary<string, string>()
    {
        {"gamespeed", "Oyun Hızı"},
        {"continue", "Devam Et"},
        {"newgame", "Yeni Oyun"},
        {"savegame", "Oyunu Kaydet"},
        {"loadgame", "Oyun Yükle"},
        {"quit", "Çıkış"},
        {"gamemenu", "Oyun Menüsü"},
        {"selectscenario", "Bir senaryo seçin"},
        {"selectteam", "Takımınızı seçin"},
        {"treasure", "Hazine: {0}"},
        {"population", "Nüfus: {0}"},
        {"militarydegree", "Askeri Gelişim: +{0}%"},
        {"populationgrowth", "Nüfus Artışı: +{0}%"},
        {"taxrate", "Vergi Artışı: +{0}%"},
        {"raisetroop", "Birlik Topla"},
        {"growpopulation", "Nüfusu Büyüt"},
        {"improveeconomy", "Ekonomiyi Geliştir"},
        {"cost", "{0} ALTIN, {1} NÜFUS"},
        {"status", "Durum: {0}"},
        {"Wait", "Bekle"},
        {"Move", "Git"},
        {"Attack", "Saldır"},
        {"Defend", "Savun"},
        {"Split", "Böl"},
        {"retreat", "Geri Çekil"},
        {"gamesaved", "Oyun başarıyla kaydedildi."},
        {"nosave", "Kayıt bulunamadı."},
        {"calltroops", "Ordu Çağrısı"},
        {"excludefighting", "Çatışanlar Hariç"},
        {"onlyhighmorale", "Yalnızca Yüksek Moral"},
        {"call", "Çağır"},
        {"directedunits", "{0} adet birlik {1} bölgesine yönlendirildi."},
        {"Europe 1500", "1500'ler Avrupası"},
        {"Warring States Of China","Çin'in Savaşan Devletleri"},
        {"Ottoman Empire", "Osmanlı İmparatorluğu"},
        {"Hungary", "Macaristan"},
        {"Holy Roman Empire", "Kutsal Roma İmparatorluğu"},
        {"France", "Fransa"},
        {"Britain", "Büyük Britanya"},
        {"Castile", "Kastilya"},
        {"Venice", "Venedik"},
        {"Poland-Lithuania", "Lehistan"},
        {"Naples", "Napoli"},
        {"Kalmar Union", "Kalmar Birliği"},
        {"Crimea", "Kırım Hanlığı"},
        {"Portugal", "Portekiz"},
        {"Russia", "Rus Çarlığı"},
        {"Armenia", "Ermenistan"},
        {"Azerbaijan", "Azerbaycan"},
        {"Caucasus Conflict", "Kafkasya Çatışması"},
        {"Divided Island", "Bölünmüş Ada"},
        {"Syrian Civil War", "Suriye İç Savaşı"},
        {"Goverment Forces", "Hükümet Güçleri"},
        {"Rebel Forces", "Muhalif Güçler"},
        {"Kurdish Forces", "YPG"},
        {"ISIS", "IŞİD"},
        {"won", "{0} kazandı!"},
        {"thanks", "Desteğiniz için Teşekkürler!"}
    };

    public Dictionary<string, string> RU = new Dictionary<string, string>()
    {
        {"gamespeed", "Скорость игры"},
        {"continue", "Продолжать"},
        {"newgame", "Новая игра"},
        {"savegame", "Сохраните игру"},
        {"loadgame", "Загрузите игру"},
        {"quit", "Выйти из игры"},
        {"gamemenu", "Игровое меню"},
        {"selectscenario", "Выберите сценарий"},
        {"selectteam", "Выбери свою команду"},
        {"treasure", "Сокровище: {0}"},
        {"population", "население: {0}"},
        {"militarydegree", "Военная степень: +{0}%"},
        {"populationgrowth", "Рост населения: +{0}%"},
        {"taxrate", "Ставка налога: +{0}%"},
        {"raisetroop", "Поднять отряд"},
        {"growpopulation", "Рост населения"},
        {"improveeconomy", "улучшить экономику"},
        {"cost", "{0} золото, {1} население"},
        {"status", "статус: {0}"},
        {"wait", "Ждать"},
        {"move", "Двигаться"},
        {"attack", "Атака"},
        {"defend", "Защищать"},
        {"split", "Расколоть"},
        {"retreat", "отступление"},
        {"gamesaved", "Игра успешно сохранена."},
        {"nosave", "Сохранений не найдено."},
        {"calltroops", "Вызов войск"},
        {"excludefighting", "Исключенные бои"},
        {"onlyhighmorale", "Только высокая мораль"},
        {"call", "Вызов"},
        {"directedunits", "{0} войск были отправлены к {1}."},
        {"Europe 1500", "Европа 1500"},
        {"Warring States Of China","Воюющие государства в Китае"},
        {"Ottoman Empire", "Османская империя"},
        {"Hungary", "Венгрия"},
        {"Holy Roman Empire", "Святая Римская Империя"},
        {"France", "Франция"},
        {"Britain", "Британия"},
        {"Castile", "Кастилия"},
        {"Venice", "Венеция"},
        {"Poland-Lithuania", "Польша-Литва"},
        {"Naples", "Неаполь"},
        {"Kalmar Union", "Кальмар Юнион"},
        {"Crimea", "Крым"},
        {"Portugal", "Португалия"},
        {"Russia", "Россия"},
        {"Armenia", "Армения"},
        {"Azerbaijan", "азербайджан"},
        {"Caucasus Conflict", "Кавказский конфликт"},
        {"Divided Island", "Разделенный остров"},
        {"Syrian Civil War", "Сирийская гражданская война"},
        {"Goverment Forces", "Правительственные силы"},
        {"Rebel Forces", "Повстанческие силы"},
        {"Kurdish Forces", "Курдские силы"},
        {"ISIS", "ИГИЛ"},
        {"won", "{0} выиграл!"},
        {"thanks", "Спасибо за поддержку!"}
    };

    // Start is called before the first frame update
    void Start()
    {
        //SwitchLanguage(Application.systemLanguage);
        SwitchLanguage(SystemLanguage.English);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchLanguage(SystemLanguage lang)
    {
        language = lang;
        
        foreach(UnityEngine.UI.Text text in texts)
        {
            text.text = Translate(text.text);
        }
    }

    public string Translate(string code)
    {
        string translated = code;

        try
        {
            switch (language)
            {
                case SystemLanguage.Turkish:
                    translated = TR[code];
                    break;

                case SystemLanguage.Russian:
                    translated = RU[code];
                    break;

                default:
                    translated = EN[code];
                    break;
            }
        }
        catch
        {
            Debug.Log("LANG_ERROR: " + code);
        }
        

        return translated;
    }

    public string Translate(string code, params string[] args)
    {
        string translated = code;

        try
        {
            switch (language)
            {
                case SystemLanguage.Turkish:
                    translated = string.Format(TR[code], args);
                    break;

                case SystemLanguage.Russian:
                    translated = string.Format(RU[code], args);
                    break;

                default:
                    translated = string.Format(EN[code], args);
                    break;
            }
        }
        catch
        {
            Debug.Log("LANG_ERROR: " + code);
        }

        return translated;
    }
}
