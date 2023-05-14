﻿namespace IconMasterAI.Core.Shared;

public class Error
{
    public static readonly Error None = new(string.Empty, string.Empty);

    public string Code { get; set; }
    public string Message { get; set; }

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }
}
