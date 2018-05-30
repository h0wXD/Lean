﻿/*
 * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
/*
 * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using System.Linq;
using QuantConnect.Data;
using QuantConnect.Securities.Option;

namespace QuantConnect.Algorithm.CSharp
{
    /// <summary>
    /// Demonstration of the Option Chain Provider -- a much faster mechanism for manually specifying the option contracts you'd like to recieve
    /// data for and manually subscribing to them.
    /// </summary>
    /// <meta name="tag" content="strategy example" />
    /// <meta name="tag" content="options" />
    /// <meta name="tag" content="using data" />
    /// <meta name="tag" content="selecting options" />
    /// <meta name="tag" content="manual selection" />
    public class OptionChainProviderAlgorithm : QCAlgorithm
    {
        private Symbol _equitySymbol;
        private Symbol _optionContract = string.Empty;

        public override void Initialize()
        {
            SetStartDate(2015, 12, 24);
            SetEndDate(2015, 12, 24);
            SetCash(100000);
            var equity = AddEquity("GOOG", Resolution.Minute);
            _equitySymbol = equity.Symbol;
        }

        public override void OnData(Slice data)
        {
            if (!Portfolio[_equitySymbol].Invested)
            {
                MarketOrder(_equitySymbol, 100);
            }

            if (!(Securities.ContainsKey(_optionContract) && Portfolio[_optionContract].Invested))
            {
                var contracts = OptionChainProvider.GetOptionContractList(_equitySymbol, data.Time);
                var underlyingPrice = Securities[_equitySymbol].Price;
                // filter the out-of-money call options from the contract list which expire in 10 to 30 days from now on
                var otmCalls = (from symbol in contracts
                                where symbol.ID.OptionRight == OptionRight.Call
                                where symbol.ID.StrikePrice - underlyingPrice > 0
                                where ((symbol.ID.Date - data.Time).TotalDays < 30 && (symbol.ID.Date - data.Time).TotalDays > 10)
                                select symbol);

                if (otmCalls.Count() != 0)
                {
                    _optionContract = otmCalls.OrderBy(x => x.ID.Date)
                                          .ThenBy(x => (x.ID.StrikePrice - underlyingPrice))
                                          .FirstOrDefault();
                    // use AddOptionContract() to subscribe the data for specified contract 
                    AddOptionContract(_optionContract, Resolution.Minute);
                }
                else _optionContract = string.Empty;
            }
            if (Securities.ContainsKey(_optionContract) && !Portfolio[_optionContract].Invested)
            {
                MarketOrder(_optionContract, -1);
            }
        }
    }
}