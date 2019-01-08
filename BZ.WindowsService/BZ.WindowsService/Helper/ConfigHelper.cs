using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BZ.WindowsService.Helper
{
    public class ConfigHelper
    {
        private const string SECTION_NAME = "serviceSetting";
        private static readonly Configuration config;

        static ConfigHelper()
        {
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (!config.Sections.Keys.Cast<string>().Any(a => a == SECTION_NAME))
            {
                config.Sections.Add("serviceSetting", new ServiceSettingSection());
                config.Save(ConfigurationSaveMode.Minimal);
                ConfigurationManager.RefreshSection(SECTION_NAME);
            }
            //if (!(config.Sections.Keys(SECTION_NAME) is ServiceSettingSection serviceSetting))
            //{
            //    config.Sections.Add("serviceSetting", new ServiceSettingSection());
            //    config.Save(ConfigurationSaveMode.Minimal);
            //    ConfigurationManager.RefreshSection(SECTION_NAME);
            //}
        }

        /// <summary>
        /// 服务设置
        /// </summary>
        public ServiceSettingSection ServiceSetting
        {
            get
            {
                ServiceSettingSection serviceSetting = config.GetSection(SECTION_NAME) as ServiceSettingSection;
                return serviceSetting;
            }
        }

        /// <summary>
        /// 保存修改
        /// </summary>
        public void SaveChange()
        {
            config.Save(ConfigurationSaveMode.Minimal);
            ConfigurationManager.RefreshSection(SECTION_NAME);
        }

        public class ServiceSettingSection : ConfigurationSection
        {
            [ConfigurationProperty("name", IsRequired = true, DefaultValue = "BZ.Services")]
            public string ServiceName
            {
                get
                {
                    return (string)base["name"];
                }
                set
                {
                    base["name"] = value;
                }
            }

            [ConfigurationProperty("plugins", IsDefaultCollection = false)]
            [ConfigurationCollection(typeof(PluginSection), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap, RemoveItemName = "remove")]
            public Plugins Plugins
            {
                get
                {
                    return (Plugins)base["plugins"];
                }
                set
                {
                    base["plugins"] = value;
                }
            }
        }

        public class PluginSection : ConfigurationSection
        {
            [ConfigurationProperty("name", IsRequired = true)]
            public string Name
            {
                get
                {
                    return (string)base["name"];
                }
                set
                {
                    base["name"] = value;
                }
            }

            [ConfigurationProperty("author")]
            public string Author
            {
                get
                {
                    return (string)base["author"];
                }
                set
                {
                    base["author"] = value;
                }
            }

            [ConfigurationProperty("interval", DefaultValue = (uint)0)]
            public uint Interval
            {
                get
                {
                    return (uint)base["interval"];
                }
                set
                {
                    base["interval"] = value;
                }
            }

            [ConfigurationProperty("enable", DefaultValue = false)]
            public bool Enabled
            {
                get
                {
                    return (bool)base["enable"];
                }
                set
                {
                    base["enable"] = value;
                }
            }
        }

        public class Plugins : ConfigurationElementCollection
        {
            protected override ConfigurationElement CreateNewElement()
            {
                return new PluginSection();
            }

            protected override object GetElementKey(ConfigurationElement element)
            {
                return ((PluginSection)element).Name;
            }

            public PluginSection this[int index]
            {
                get
                {
                    return (PluginSection)base.BaseGet(index);
                }
            }

            public PluginSection this[string key]
            {
                get
                {
                    return (PluginSection)base.BaseGet(key);
                }
            }

            public void Add(PluginSection plugin)
            {
                base.BaseAdd(plugin, true);
            }

            public void Remove(string key)
            {
                base.BaseRemove(key);
            }

            public string[] GetKeys()
            {
                return BaseGetAllKeys().Cast<string>().ToArray();
            }
        }
    }
}
