﻿namespace Risparmioforo.Shared.Base;


public class AppSettings
{
    public required ConnectionStrings ConnectionStrings { get; set; }
}

public class ConnectionStrings
{
    public string DefaultConnection { get; set; }

    public string TestConnection  { get; set; }
}