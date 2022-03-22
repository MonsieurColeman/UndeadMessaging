using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace delLater
{
    public static class ServiceHandler
    {
        public static string AttemptService(Func<string> fnc)
        {

                int count = 0;
                string msg = "";
                while (true)
                {
                    try
                    {
                        //msg = fnc.Invoke();
                        fnc.Invoke();
                        break;
                    }
                    catch (IndexOutOfRangeException ex)
                    {
                        Console.WriteLine("Is the same problem");
                    }
                    catch (TimeoutException timeProblem)
                    {
                        Console.WriteLine("The service operation timed out. " + timeProblem.Message);
                        continue;
                        Console.ReadLine();
                    }
                    // Catch unrecognized faults. This handler receives exceptions thrown by WCF
                    // services when ServiceDebugBehavior.IncludeExceptionDetailInFaults
                    // is set to true.
                    catch (FaultException faultEx)
                    {
                        Console.WriteLine("An unknown exception was received. "
                          + faultEx.Message
                          + faultEx.StackTrace
                        );
                        Console.Read();
                        continue;
                    }
                    // Standard communication fault handler.
                    catch (CommunicationException commProblem)
                    {
                        Console.WriteLine("There was a communication problem. " + commProblem.Message + commProblem.StackTrace);
                        Console.Read();
                        continue;
                    }
                    catch (Exception exc)
                    {
                        if (count > 4)
                        {
                            return "Max retries exceeded";
                        }
                        Console.Write("\n  {0}", exc.Message);
                        Console.Write("\n  service failed {0} times - trying again", ++count);
                        Thread.Sleep(100);
                        continue;
                    }
                }
                return msg;
            
        }
    }
}
