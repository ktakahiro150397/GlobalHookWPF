using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Serialization;
using System.IO;

namespace HookApp.Models
{
    public class SettingFileDeserializer
    {

        private string _filePath;

        public SettingFileDeserializer(string filePath)
        {
            _filePath = filePath;
        }

        /// <summary>
        /// 対象のYAML設定ファイルを指定した型でデシリアライズします。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T YAMLDeserialize<T>()
        {
            //ファイルを開き、テキストとして取得
            var fileSr = new StreamReader(_filePath);
            string text = fileSr.ReadToEnd();

            //デシリアライザを構成し、結果を返却
            Deserializer deserializer = new DeserializerBuilder().Build();
            T data = deserializer.Deserialize<T>(text);

            return data;
        }



    }
}
