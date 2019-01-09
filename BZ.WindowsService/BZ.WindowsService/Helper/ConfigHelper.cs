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
        /// <summary>
        /// 节点名称
        /// </summary>
        private const string SECTION_NAME = "serviceSetting";
        private static readonly Configuration config;

        static ConfigHelper()
        {
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            #region 初始化Section
            if (!config.Sections.Keys.Cast<string>().Any(a => a == SECTION_NAME))
            {
                config.Sections.Add(SECTION_NAME, new ServiceSettingSection());
                config.Save(ConfigurationSaveMode.Minimal);
                ConfigurationManager.RefreshSection(SECTION_NAME);
            }
            #endregion
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
            /// <summary>
            /// 服务名称
            /// </summary>
            [ConfigurationProperty("name", IsRequired = true, DefaultValue = "BZ.Services")]
            public string ServiceName
            {
                get
                {
                    return (string)base["name"];
                }
                set
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        value = "BZ.Services";
                    }
                    base["name"] = value;
                }
            }

            /// <summary>
            /// 插件配置集合
            /// </summary>
            [ConfigurationProperty("plugins", IsDefaultCollection = false)]
            [ConfigurationCollection(typeof(PluginSettingSection), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap, RemoveItemName = "remove")]
            public PluginSettings PluginSettings
            {
                get
                {
                    return (PluginSettings)base["plugins"];
                }
                set
                {
                    base["plugins"] = value;
                }
            }
        }

        /// <summary>
        /// 插件配置节点
        /// </summary>
        public class PluginSettingSection : ConfigurationSection
        {
            /// <summary>
            /// 插件名称
            /// </summary>
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

            /// <summary>
            /// 作者
            /// </summary>
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

            /// <summary>
            /// 运行间隔
            /// </summary>
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

            /// <summary>
            /// 是否启用
            /// </summary>
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

        /// <summary>
        /// 插件配置节点集合
        /// </summary>
        public class PluginSettings : ConfigurationElementCollection
        {
            protected override ConfigurationElement CreateNewElement()
            {
                return new PluginSettingSection();
            }

            protected override object GetElementKey(ConfigurationElement element)
            {
                return ((PluginSettingSection)element).Name;
            }

            public PluginSettingSection this[int index]
            {
                get
                {
                    return (PluginSettingSection)BaseGet(index);
                }
            }

            public new PluginSettingSection this[string key]
            {
                get
                {
                    return (PluginSettingSection)BaseGet(key);
                }
            }

            /// <summary>
            /// 添加新插件配置项
            /// </summary>
            /// <param name="plugin"></param>
            public void Add(PluginSettingSection plugin)
            {
                BaseAdd(plugin, true);
            }

            /// <summary>
            /// 移除插件配置项
            /// </summary>
            /// <param name="key"></param>
            public void Remove(string key)
            {
                BaseRemove(key);
            }

            /// <summary>
            /// 获取所有配置项的Key
            /// </summary>
            /// <returns></returns>
            public string[] GetKeys()
            {
                return BaseGetAllKeys().Cast<string>().ToArray();
            }
        }
    }
}
