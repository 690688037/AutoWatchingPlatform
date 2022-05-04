using Caliburn.Micro;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using AutoWatchingPlatform.Model;
using System.Threading;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Input;
using AutoWatchingPlatform.Model.PO;
using System.Windows.Threading;
using System.Text;

namespace AutoWatchingPlatform
{
    [Export(typeof(IMainWindow))]
    public class MainWindowViewModel : PropertyChangedBase
    {
        public MainWindowViewModel()
        {
            watchingList = desrializeList<ObservableCollection<WatchingData>>(Properties.Settings.Default.watchingList);
            sharesList = desrializeList<ObservableCollection<SharesData>>(Properties.Settings.Default.sharesList);
            priceList = desrializeList<ObservableCollection<PriceData>>(Properties.Settings.Default.priceList);
            watchingIndex = Properties.Settings.Default.watchingIndex;
            sharesIndex = Properties.Settings.Default.sharesIndex;
            priceIndex = Properties.Settings.Default.priceIndex;

            loadMainWindowBackEnd();
        }

        private enum SharesGridType
        {
            Shares,
            Watching,
            Price
        }

        private WindowState _windowsState;
        public WindowState windowsState
        {
            get
            {
                return _windowsState;
            }
            set
            {
                _windowsState = value;
                NotifyOfPropertyChange("windowsState");
            }
        }

        private bool _isSimpleMode = false;
        public bool isSimpleMode
        {
            get { return _isSimpleMode; }
            set
            {
                _isSimpleMode = value;
                NotifyOfPropertyChange("isSimpleMode");
            }
        }

        private string _code = "";
        public string code
        {
            get { return _code; }
            set
            {
                _code = value;
                NotifyOfPropertyChange("code");
            }
        }

        private string _value = "";
        public string value
        {
            get { return _value; }
            set
            {
                _value = value;
                NotifyOfPropertyChange("value");
            }
        }

        private bool _isSky = true;
        public bool isSky
        {
            get { return _isSky; }
            set
            {
                _isSky = value;
                NotifyOfPropertyChange("isSky");
            }
        }

        private bool _isLand = false;
        public bool isLand
        {
            get { return _isLand; }
            set
            {
                _isLand = value;
                NotifyOfPropertyChange("isLand");
            }
        }

        private ObservableCollection<WatchingData> _watchingList = new ObservableCollection<WatchingData>();
        public ObservableCollection<WatchingData> watchingList
        {
            get { return _watchingList; }
            set
            {
                _watchingList = value;
                NotifyOfPropertyChange("watchingList");
            }
        }

        private ObservableCollection<SharesData> _sharesList = new ObservableCollection<SharesData>();
        public ObservableCollection<SharesData> sharesList
        {
            get { return _sharesList; }
            set
            {
                _sharesList = value;
                NotifyOfPropertyChange("sharesList");
            }
        }
        private SharesData _selectedShares;
        public SharesData selectedShares
        {
            get { return _selectedShares; }
            set
            {
                _selectedShares = value;
                NotifyOfPropertyChange("selectedShares");
            }
        }

        private ObservableCollection<PriceData> _priceList = new ObservableCollection<PriceData>();
        public ObservableCollection<PriceData> priceList
        {
            get { return _priceList; }
            set
            {
                _priceList = value;
                NotifyOfPropertyChange("priceList");
            }
        }
        private PriceData _selectedPrice;
        public PriceData selectedPrice
        {
            get { return _selectedPrice; }
            set
            {
                _selectedPrice = value;
                NotifyOfPropertyChange("selectedPrice");
            }
        }

        AllShares allShares;
        static long watchingIndex = 1;
        static long sharesIndex = 1;
        static long priceIndex = 1;
        //static long secondBeatBoardIndex = 1;

        bool isLoad = false;

        public void add()
        {
            var dataList = getSharesData(code);
            if (dataList == null)
            {
                MessageBox.Show("Cannot find data");
                return;
            }
            var watchingData = new WatchingData(watchingIndex++, dataList[1], dataList[2],isSky? WatchingData.BoardType.sky:WatchingData.BoardType.land, value);
            watchingList.Add(watchingData);
            //增加当前数量
            var watchingDataForRecord = watchingData.Clone();
            watchingDataForRecord.amount += "+" + (isSky ? dataList[10] : dataList[20]);
            ThreadPool.QueueUserWorkItem(createWatching, new InputData(watchingData.id, code, isSky, value));
        }

        public void delete()
        {
            isCancleAllWatching = true;//暂时取消多个线程
            watchingList.Clear();
        }

        bool isCancleAllWatching = false;
        private void createWatching(object state)
        {
            var sharesDate = (InputData)state;
            string realName = string.Empty;
            try
            {
                while (!isCancleAllWatching)
                {
                    var dataList = getSharesData(sharesDate.code, realName);
                    realName = dataList[1];

                    if (sharesDate.isSky)
                    {
                        if (Convert.ToInt32(dataList[10]) <= Convert.ToInt32(sharesDate.amount))
                        {
                            MessageBox.Show(DateTime.Now + "\n" + dataList[1] + "[" + dataList[2] + "]" + " is breaking from sky" + "\n" + "buyDetail is less than " + sharesDate.amount
                                , "Sky is breaking", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                            callBackForWatching(sharesDate.id);
                            return;
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(dataList[20]) <= Convert.ToInt32(sharesDate.amount))
                        {
                            MessageBox.Show(DateTime.Now + "\n" + dataList[1] + "[" + dataList[2] + "]" + "is breaking from broad." + "\n" + "sellDetail is less than " + sharesDate.amount
                                , "Land is breaking", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                            callBackForWatching(sharesDate.id);
                            return;
                        }
                    }
                    Thread.Sleep(CommonMethod.quicklyIntervalTime);
                }

                Thread.Sleep(CommonMethod.quicklyIntervalTime);
                isCancleAllWatching = false;//1s后重置取消开板检测标志
            }
            catch
            {
                //新开线程以重新工作
                ThreadPool.QueueUserWorkItem(createWatching, state);
            }
        }

        public void addShares()
        {
            var dataList = getSharesData(code);
            if (dataList == null)
            {
                MessageBox.Show("Cannot find data");
                return;
            }
            var sharesData = new SharesData(sharesIndex++, dataList[1], dataList[2], dataList[3], dataList[32], dataList[10], dataList[20], dataList[47], dataList[48]);
            if (sharesList.FirstOrDefault(x => x.name == dataList[1]) == null)
            {
                sharesList.Add(sharesData);
            }
            else
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("List contains this shares, are you sure?", "Repeat opertion prompt", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (messageBoxResult == MessageBoxResult.OK)
                {
                    sharesList.Add(sharesData);
                }
            }
        }

        public void deleteShares()
        {
            sharesList.Remove(selectedShares);
        }

        private void createShares(object state)
        {
            try
            {
                for (int i = sharesList.Count - 1; i >= 0; i--)
                {
                    var dataList = getSharesData(sharesList[i].code, sharesList[i].name);
                    sharesList[i].skyPrice = dataList[47];
                    sharesList[i].landPrice = dataList[48];
                }
                while (true)
                {
                    if (windowsState != WindowState.Minimized && sharesList != null)
                    {
                        for (int i = sharesList.Count - 1; i >= 0; i--)
                        {
                            var dataList = getSharesData(sharesList[i].code, sharesList[i].name);
                            sharesList[i].price = dataList[3];
                            sharesList[i].range = dataList[32];

                            StringBuilder buyDetailBuilder = new StringBuilder("", 80);
                            buyDetailBuilder.Append(dataList[9]);
                            buyDetailBuilder.Append("-");
                            buyDetailBuilder.Append(dataList[10]);
                            buyDetailBuilder.Append("  ");
                            buyDetailBuilder.Append(dataList[11]);
                            buyDetailBuilder.Append("-");
                            buyDetailBuilder.Append(dataList[12]);
                            buyDetailBuilder.Append("  ");
                            buyDetailBuilder.Append(dataList[13]);
                            buyDetailBuilder.Append("-");
                            buyDetailBuilder.Append(dataList[14]);
                            buyDetailBuilder.Append("  ");
                            buyDetailBuilder.Append(dataList[15]);
                            buyDetailBuilder.Append("-");
                            buyDetailBuilder.Append(dataList[16]);
                            buyDetailBuilder.Append("  ");
                            buyDetailBuilder.Append(dataList[17]);
                            buyDetailBuilder.Append("-");
                            buyDetailBuilder.Append(dataList[18]);
                            sharesList[i].buyDetail = buyDetailBuilder.ToString();


                            StringBuilder sellDetailBuilder = new StringBuilder("", 80);
                            sellDetailBuilder.Append(dataList[19]);
                            sellDetailBuilder.Append("-");
                            sellDetailBuilder.Append(dataList[20]);
                            sellDetailBuilder.Append("  ");
                            sellDetailBuilder.Append(dataList[21]);
                            sellDetailBuilder.Append("-");
                            sellDetailBuilder.Append(dataList[22]);
                            sellDetailBuilder.Append("  ");
                            sellDetailBuilder.Append(dataList[23]);
                            sellDetailBuilder.Append("-");
                            sellDetailBuilder.Append(dataList[24]);
                            sellDetailBuilder.Append("  ");
                            sellDetailBuilder.Append(dataList[25]);
                            sellDetailBuilder.Append("-");
                            sellDetailBuilder.Append(dataList[26]);
                            sellDetailBuilder.Append("  ");
                            sellDetailBuilder.Append(dataList[27]);
                            sellDetailBuilder.Append("-");
                            sellDetailBuilder.Append(dataList[28]);
                            sharesList[i].sellDetail = sellDetailBuilder.ToString();
                        }
                    }

                    Thread.Sleep(CommonMethod.quicklyIntervalTime);
                }
            }
            catch
            {
                //新开线程以重新工作
                ThreadPool.QueueUserWorkItem(createShares);
            }
        }

        public void addPrice()
        {
            if (string.IsNullOrEmpty(value))
            {
                MessageBox.Show("Input Target Price in value");
                return;
            }
            var dataList = getSharesData(code);
            if (dataList == null)
            {
                MessageBox.Show("Cannot find data");
                return;
            }
            var priceData = new PriceData(priceIndex++, dataList[1], dataList[2], value,
                (Convert.ToDouble(value) / Convert.ToDouble(dataList[5]) - 1.0).ToString("0.00"),
                isSky ? PriceData.ComparisonOperator.bigger : PriceData.ComparisonOperator.smaller);
            priceList.Add(priceData);
        }

        public void deletePrice()
        {
            priceList.Remove(selectedPrice);
        }

        private void createPrice(object state)
        {
            try
            {
                while (true)
                {
                    if (priceList != null)
                    {
                        for (int i = priceList.Count - 1; i >= 0; i--)
                        {
                            var dataList = getSharesData(priceList[i].code, priceList[i].name);
                            if ((priceList[i].comparisonOperator.Equals(PriceData.ComparisonOperator.bigger) && Convert.ToDouble(dataList[3]) >= Convert.ToDouble(priceList[i].price)))
                            {
                                MessageBox.Show(DateTime.Now + "\n" + dataList[1] + "[" + dataList[2] + "]" + "'s price is bigger than " + priceList[i].price
                                    , "price arrived (bigger)", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                                callBackForPrice(priceList[i].id);
                                continue;
                            }
                            else if((priceList[i].comparisonOperator.Equals(PriceData.ComparisonOperator.smaller) && Convert.ToDouble(dataList[3]) <= Convert.ToDouble(priceList[i].price)))
                            {
                                MessageBox.Show(DateTime.Now + "\n" + dataList[1] + "[" + dataList[2] + "]" + "'s price is smaller than " + priceList[i].price
                                    , "price arrived (smaller)", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                                callBackForPrice(priceList[i].id);
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    Thread.Sleep(CommonMethod.quicklyIntervalTime);
                }
            }
            catch
            {
                //新开线程以重新工作
                ThreadPool.QueueUserWorkItem(createPrice);
            }
        }


        private void syncAllShares(object state)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(SharesApi.getAllShares);
                var response = (HttpWebResponse)request.GetResponse();
                var dataList = new StreamReader(response.GetResponseStream()).ReadToEnd();//可以省略：, System.Text.Encoding.UTF8

                allShares = JsonConvert.DeserializeObject<AllShares>(Properties.Settings.Default.allShares);

                Properties.Settings.Default.allShares = dataList;
                Properties.Settings.Default.Save();
            }
            catch
            {

            }
        }

        private string[] getSharesData(string code, string name = "")
        {
            string realCode = code;
            string realName = name;
            if (CommonMethod.HashChinese(code))
            {
                realCode = allShares.data.diff.FirstOrDefault(x => x.f14 == code)?.f12;
                if (realCode == null)
                {
                    return null;
                }
                realName = code;
            }

            if (realCode.Length <= 6)
            {
                var combineCode = getSharesDataFromApi("sh" + realCode);
                if (combineCode == null || (combineCode != null && !string.IsNullOrEmpty(realName) && combineCode[1] != realName))
                {
                    return getSharesDataFromApi("sz" + realCode);
                }
                else
                {
                    return combineCode;
                }
            }
            else
            {
                return getSharesDataFromApi(realCode);
            }
        }

        private string[] getSharesDataFromApi(string code)
        {
            var request = (HttpWebRequest)WebRequest.Create(string.Format(SharesApi.getSharesDataByCode, code));
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default).ReadToEnd();
            var dataList = responseString.Split('~');
            if (dataList.Length < 33)
            {
                return null;
            }
            else
            {
                return dataList;
            }
        }

        private T desrializeList<T>(string property) where T : new()
        {
            if (String.IsNullOrEmpty(property))
            {
                return new T();
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(property);
            }
        }

        private void callBackForWatching(long id)
        {
            try
            {
                App.Current.Dispatcher.BeginInvoke(new System.Action(() =>
                {
                    watchingList.Remove(watchingList.First(x => x.id == id));
                }));
            }
            catch
            {

            }
        }

        private void callBackForPrice(long id)
        {
            try
            {
                App.Current.Dispatcher.BeginInvoke(new System.Action(() =>
                {
                    priceList.Remove(priceList.First(x => x.id == id));
                }));
            }
            catch
            {

            }
        }

        private int getAvailableWorkerThreads()
        {
            int availableWorkerThreads;
            int availableCompletionPortThreads;
            ThreadPool.GetAvailableThreads(out availableWorkerThreads, out availableCompletionPortThreads);
            return availableWorkerThreads;
        }

        private void loadMainWindowBackEnd()
        {
            isLoad = true;

            ThreadPool.SetMaxThreads(20, 50);

            //加载开板信息
            foreach (var watchData in watchingList)
            {
                ThreadPool.QueueUserWorkItem(createWatching, new InputData(watchData.id, watchData.code,
                    watchData.boardType.Equals(WatchingData.BoardType.sky), watchData.amount));
            }
            //信息刷新
            ThreadPool.QueueUserWorkItem(createShares);
            //价格刷新
            ThreadPool.QueueUserWorkItem(createPrice);
            ////二次打板刷新
            //ThreadPool.QueueUserWorkItem(createSecondBeatBoard);
            //同步最新信息
            ThreadPool.QueueUserWorkItem(syncAllShares);
            //自动保存数据
            ThreadPool.QueueUserWorkItem(saveSetting,"AutoSave");
        }

        private void saveSetting(object state)
        {
            while (true)
            {
                //Properties.Settings.Default.width = width;
                //Properties.Settings.Default.height = height;
                Properties.Settings.Default.watchingList = JsonConvert.SerializeObject(watchingList);
                Properties.Settings.Default.sharesList = JsonConvert.SerializeObject(sharesList);
                Properties.Settings.Default.priceList = JsonConvert.SerializeObject(priceList);
                //Properties.Settings.Default.secondBeatBoardList = JsonConvert.SerializeObject(secondBeatBoardList);
                Properties.Settings.Default.watchingIndex = watchingIndex;
                Properties.Settings.Default.sharesIndex = sharesIndex;
                Properties.Settings.Default.priceIndex = priceIndex;
                //Properties.Settings.Default.secondBeatBoardIndex = secondBeatBoardIndex;
                Properties.Settings.Default.Save();

                if (state.ToString() == "AutoSave")
                {
                    Thread.Sleep(60000);//每隔一分钟保存一次配置
                }
                else
                {
                    break;//目前只有Closing
                }
            }
        }

        public void CodeKeyDownHandler(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (e.KeyboardDevice.Modifiers == ModifierKeys.Shift)
                {
                    add();
                }
                else if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
                {
                    addPrice();
                }
                else
                {
                    addShares();
                }
            }
        }
        public void ClosingHandler()
        {
            saveSetting("Closing");
        }
    }
}
