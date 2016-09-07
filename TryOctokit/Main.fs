module TryOctokit.Main

open System
open NUnit.Framework
open FluentAssertions
open Octokit
open System.Linq
open System.Collections.Generic
open System.IO
open System.IO.Compression
open System.Text

let user =  Environment.GetEnvironmentVariable("ghu");
let pass =  Environment.GetEnvironmentVariable("ghp"); 

[<Test>]
let shouldReadEnvVars() =
      user.Should().Be("wk-j", "") |> ignore

[<Test>]
let shouldDownloadAsset() =
      let client = new GitHubClient(new ProductHeaderValue("my-cool-app"));
      let tokenAuth = new Credentials(pass);
      client.Credentials <- tokenAuth

      let release = client.Repository.Release.GetAll("bcircle", "easy-capture").Result
      release.Count.Should().BeGreaterThan(5, "") |> ignore

      let asset = release.ElementAt(0).Assets.ElementAt(0);

      let response =  client.Connection.Get<byte[]>(new Uri(asset.Url), Dictionary<string, string>(), "application/octet-stream").Result
      do
          use outputFile = new System.IO.FileStream(asset.Name ,System.IO.FileMode.Create)
          outputFile.Write(response.Body, 0, response.Body.Length) 

     

