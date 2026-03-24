using System;
using CommandLine;
using NwRfcNetCore;

namespace Sample.RfcConnections;

class Options
{
	[Option('u', "username", Required = true, HelpText = "RFC User Name")]
	public string UserName { get; set; }

	[Option('p', "password", Required = true, HelpText = "RFC User Password")]
	public string Password { get; set; }

	[Option('h', "hostname", Required = true, HelpText = "RFC Server Hostname")]
	public string Hostname { get; set; }

	[Option('c', "client", Required = true, HelpText = "RFC Server Client Id")]
	public string Client { get; set; }
}

class Program
{
	static void Main(string[] args)
	{
		Parser.Default.ParseArguments<Options>(args)
			   .WithParsed(o =>
			   {
				   var version = RfcConnection.GetLibVersion();
				   Console.WriteLine($"currently loaded sapnwrfc library version : Major {version.MajorVersion}, Minor {version.MinorVersion}, patchLevel {version.PatchLevel}");

				   /**
					* Use the connection builder fluent api.
					*/
				   using var conn = RfcConnection.FromBuilder(builder => builder
						.UseConnectionHost(o.Hostname)
						.UseLogonUserName(o.UserName)
						.UseLogonPassword(o.Password)
						.UseLogonClient(o.Client));

				   conn.Open();
				   conn.Ping();
				   // do something more interesting here...

				   /**
					* Use the connection string api.
					*/
				   using var conn2 = RfcConnection.FromConnectionString($"Server={o.Hostname}; Uid={o.UserName}; Passwd={o.Password}; Client={o.Client}");
				   conn2.Open();
				   conn2.Ping();
				   // do something more interesting here...


				   /**
					* Use the connection uri api.
					*/
				   using var conn3 = RfcConnection.FromUri(new Uri($"sap://user={Uri.EscapeDataString(o.UserName)};passwd={Uri.EscapeDataString(o.Password)};client={o.Client}@A/{o.Hostname}"));
				   conn3.Open();
				   conn3.Ping();
				   // do something more interesting here...				   

			   });
	}
}
