using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MasterMind
{
    public class HighscoreList : Dictionary<string, Highscore>, IXmlSerializable
    {
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void SetHighscore(string p_userName, int p_Won)
        {

        }

        public int GetHighScore(string p_username)
        {
            return 0;
        }

        public void ReadXml(XmlReader reader)
        {
            while (reader.Read() &&
                !(reader.NodeType == XmlNodeType.EndElement && reader.LocalName == this.GetType().Name))
            {
                var name = reader["PlayerName"];
                if (name == null)
                    throw new FormatException();

                Highscore value = new Highscore() { PlayerName = reader["PlayerName"], Played = int.Parse(reader["Played"]), Won = int.Parse(reader["Won"]) };
                this[name] = value;
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            foreach (KeyValuePair<string,Highscore> entry in this)
            {
                writer.WriteStartElement("Highscore");
                writer.WriteAttributeString("PlayerName", entry.Key);
                writer.WriteAttributeString("Played", entry.Value.Played.ToString());
                writer.WriteAttributeString("Won", entry.Value.Won.ToString());
                writer.WriteEndElement();
            }
        }
    }
}
