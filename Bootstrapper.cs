using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;

namespace AutoWatchingPlatform
{
    public class Bootstrapper : Bootstrapper<IMainWindow>
    {
        //public Bootstrapper()
        //{

        //}

        //protected override void OnStartup(object sender, StartupEventArgs e)
        //{
        //    DisplayRootViewFor<MainViewModel>();
        //}

        //公共方法
        private CompositionContainer _container;
        //用MEF组合部件
        protected override void Configure()
        {
            _container = new CompositionContainer(
                new AggregateCatalog(AssemblySource.Instance.Select(x => new AssemblyCatalog(x)).OfType<ComposablePartCatalog>()));

            ///如果还有自己的部件都加在这个地方
            CompositionBatch _batch = new CompositionBatch();
            _batch.AddExportedValue<IWindowManager>(new WindowManager());
            _batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            //_batch.AddExportedValue<IDownloadHelper>(new DownloadHelper());
            _batch.AddExportedValue(_container);


            _container.Compose(_batch);
        }
        //根据传过来的key或名称得到实例
        protected override object GetInstance(Type service, string key)
        {
            string _contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(service) : key;

            var _exports = _container.GetExportedValues<object>(_contract);

            if (_exports.Any())
            {
                return _exports.First();
            }
            throw new Exception(string.Format("找不到{0}实例", _contract));
        }
        //获取某一特定类型的所有实例
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetExportedValues<object>(AttributedModelServices.GetContractName(service));
        }
        //将实例传递给 Ioc 容器，使依赖关系注入
        protected override void BuildUp(object instance)
        {
            _container.SatisfyImportsOnce(instance);
        }
    }

    public interface IMainWindow
    {
    }
}
