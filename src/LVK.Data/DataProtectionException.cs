﻿namespace LVK.Data;

public class DataProtectionException : InvalidOperationException
{
    public DataProtectionException(string message)
        : base(message)
    {
    }

    public DataProtectionException(string message, Exception? innerException)
        : base(message, innerException)
    {
    }
}