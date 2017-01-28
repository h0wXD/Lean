﻿/*
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

using QuantConnect.Data;
using QuantConnect.Securities;

namespace QuantConnect.Orders.Slippage
{
    /// <summary>
    /// Represents a slippage model that uses a constant percentage of slip
    /// </summary>
    public class ZeroSlippageModel : ISlippageModel
    {

        /// <summary>
        /// Slippage Model. Return a decimal cash slippage approximation on the order.
        /// </summary>
        public decimal GetSlippageApproximation(Security asset, Order order)
        {
            return 0;
        }
    }
}
