using ResourceSearcher.UILogic.ViewModels;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceSearcher.UILogic
{
    public class ViewManager : Stylet.ViewManager
    {
        public ViewManager(ViewManagerConfig config) : base(config)
        {
            var types = config.ViewAssemblies.SelectMany(a => a.ExportedTypes);
            var baseModelType = typeof(BaseViewModel);
            VMToVDict = types.Where(type => baseModelType.IsAssignableFrom(type) && baseModelType != type).ToDictionary(type => type, type => types.FirstOrDefault(t => $"{t.Name}ViewModel" == type.Name)).Where(d => d.Value != null).ToDictionary(d => d.Key, d => d.Value);
        }

        public Dictionary<Type, Type> VMToVDict { get; }

        protected override Type LocateViewForModel(Type modelType)
        {
            if (VMToVDict.ContainsKey(modelType))
            {
                var viewType = VMToVDict[modelType];
                if (viewType != null)
                {
                    return viewType;
                }
            }
            return base.LocateViewForModel(modelType);
        }
    }
}
