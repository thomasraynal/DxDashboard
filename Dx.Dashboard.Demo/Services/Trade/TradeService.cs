using Dx.Dashboard.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dx.Dashboard.Demo
{
    public class TradeService
    {
        public ITrade CreateTrade(IStrategy strategy)
        {
            var asset = TestHelper.Random(TestHelper.Assets);
            var counterparty = TestHelper.Random(TestHelper.Counterparty);
            var amount = TestHelper.Rand.Next(50, 200);
            var price = _priceService.All.Items.First(prc => prc.Asset.Name == asset.Name);

            return new Trade(
                amount,
                price.Value,
                asset.Name,
                counterparty,
                TestHelper.Rand.Next(0, 2) == 1 ? BuySell.Buy : BuySell.Sell
                );
        }

        private TradeService()
        {
            _priceService = AppCore.Instance.Get<IEntityService<IPrice>>();
        }

        private static Lazy<TradeService> _lazyInstance;
        private IEntityService<IPrice> _priceService;

        public static TradeService Instance
        {
            get
            {
                if (null == _lazyInstance)
                {
                    var service = new TradeService();
                    _lazyInstance = new Lazy<TradeService>(() => service, LazyThreadSafetyMode.ExecutionAndPublication);
                }

                return _lazyInstance.Value;
            }
        }
    }
}
