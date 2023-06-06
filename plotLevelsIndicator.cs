#region Using declarations
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Gui.SuperDom;
using NinjaTrader.Gui.Tools;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;
using NinjaTrader.NinjaScript.Indicators;
using NinjaTrader.Core.FloatingPoint;
using NinjaTrader.NinjaScript.DrawingTools;
using System.Timers;
using System.Net;
using System.Net.Http;
using System.IO;
#endregion

//This namespace holds Indicators in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Indicators
{
	public class plotLevelsIndicator : Indicator
	{
		private DateTime lastUpdate = DateTime.MinValue;
		private string[] lines;
		private System.Timers.Timer timer;
		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description									= @"Enter the description for your new custom Indicator here.";
				Name										= "plotLevelsIndicator";
				Calculate									= Calculate.OnBarClose;
				IsOverlay									= false;
				DisplayInDataBox							= true;
				DrawOnPricePanel							= true;
				DrawHorizontalGridLines						= true;
				DrawVerticalGridLines						= true;
				PaintPriceMarkers							= true;
				ScaleJustification							= NinjaTrader.Gui.Chart.ScaleJustification.Right;
				//Disable this property if your indicator requires custom values that cumulate with each new market data event. 
				//See Help Guide for additional information.
				IsSuspendedWhileInactive					= true;
			}
			else if (State == State.Configure)
			{
				lastUpdate = DateTime.MinValue;

			}
			else if (State == State.DataLoaded)
	        {
	             // Set up timer to check for updates every 10 minutes
	            timer = new System.Timers.Timer(10 * 60 * 1000);
	            timer.Elapsed += OnTimerElapsed;
	            timer.AutoReset = true;
	            timer.Enabled = true;

	            // Initialize lastUpdate to a very old time so that the first update will always run
	            lastUpdate = DateTime.MinValue;

	            // Initialize the cache
	            updateCache();
	        }
		}
		
		
		
	 private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
	    {
	        updateCache();
	    }
	private void updateCache()
	    {
	        // Download file and parse its contents here
			// Set the initial URL based on the current date
	        string url = "https://levels.yuda.men/levels.txt";
	        using (WebClient client = new WebClient())
	        {
	            string content = client.DownloadString(url);
	            lines = content.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
	        }
	        lastUpdate = DateTime.Now;
	    }
		
	 protected override void OnBarUpdate()
    {
            foreach (string line in lines)
            {
                string[] parts = line.Split('#');
                double price;
                if (parts.Length == 2 && double.TryParse(parts[0], out price))
                {
                    string[] textParts = parts[1].Split('"');
                    if (textParts.Length == 3)
                    {
                        string color = textParts[0].Trim();
                        string text = textParts[1].Trim();
                        switch (color.ToLower())
                        {
                            case "orange":
                                Draw.HorizontalLine(this, "Level" + price, price, Brushes.Orange);
                                Draw.Text(this, "Label" + price, text, 0, price, Brushes.Orange);
                                break;
                            case "steelblue":
                                Draw.HorizontalLine(this, "Level" + price, price, Brushes.SteelBlue);
                                Draw.Text(this, "Label" + price, text, 0, price, Brushes.SteelBlue);
                                break;
							case "magenta":
		                        Draw.HorizontalLine(this, "Level" + price, price, Brushes.Magenta);
		                        Draw.Text(this, "Label" + price, text, 0, price, Brushes.Magenta);
		                        break;
                            // add more cases for other colors as needed
                        }
                    }
                }
            }

    }
}

	
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private plotLevelsIndicator[] cacheplotLevelsIndicator;
		public plotLevelsIndicator plotLevelsIndicator()
		{
			return plotLevelsIndicator(Input);
		}

		public plotLevelsIndicator plotLevelsIndicator(ISeries<double> input)
		{
			if (cacheplotLevelsIndicator != null)
				for (int idx = 0; idx < cacheplotLevelsIndicator.Length; idx++)
					if (cacheplotLevelsIndicator[idx] != null &&  cacheplotLevelsIndicator[idx].EqualsInput(input))
						return cacheplotLevelsIndicator[idx];
			return CacheIndicator<plotLevelsIndicator>(new plotLevelsIndicator(), input, ref cacheplotLevelsIndicator);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.plotLevelsIndicator plotLevelsIndicator()
		{
			return indicator.plotLevelsIndicator(Input);
		}

		public Indicators.plotLevelsIndicator plotLevelsIndicator(ISeries<double> input )
		{
			return indicator.plotLevelsIndicator(input);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.plotLevelsIndicator plotLevelsIndicator()
		{
			return indicator.plotLevelsIndicator(Input);
		}

		public Indicators.plotLevelsIndicator plotLevelsIndicator(ISeries<double> input )
		{
			return indicator.plotLevelsIndicator(input);
		}
	}
}

#endregion
