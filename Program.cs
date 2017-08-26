﻿using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;

namespace csharp_rest_client
{
  class Program
  {
    static void Main(string[] args)
    {
      ProcessRepos().Wait();
    }

    private static async Task ProcessRepos()
    {
      var client = new HttpClient();
      client.DefaultRequestHeaders.Accept.Clear();
      client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json")
        );
      client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

      var serializer = new DataContractJsonSerializer(
        typeof(List<repo>)
        );

      var streamTask = client.GetStreamAsync("https://api.github.com/orgs/dotnet/repos");

      var repositories = serializer.ReadObject(await streamTask) as List<repo>;

      foreach (var repo in repositories)
      {
        Console.WriteLine(repo.name);
      }
    }
  }
}
