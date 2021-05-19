using System;

public interface IInvokerMono
{
    void CallInvokingEveryMethod(Action method, float time);
    void CallInvokingMethod(Action method, float time);
}