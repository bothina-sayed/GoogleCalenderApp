using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalenderApplication.Domain.DTOs
{
    public class ResponseModel<T>
    {
        public bool Ok { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public static ResponseModel<T> Success(T data)
        {
            return new ResponseModel<T>
            {
                Data = data,
                Ok = true,
                Message = "success",
            };
        }
        public static ResponseModel<T> Success(string message = "success")
        {
            return new ResponseModel<T>
            {
                Data = default(T),
                Ok = true,
                Message = message,
            };
        }
        public static ResponseModel<T> Success(string message, T data)
        {
            return new ResponseModel<T>
            {
                Data = data,
                Ok = true,
                Message = message,
            };
        }
        public static ResponseModel<T> Error(string message = "an error occured")
        {
            return new ResponseModel<T>
            {
                Data = default(T),
                Ok = false,
                Message = message
            };
        }
        public static ResponseModel<T> Error(T data)
        {
            return new ResponseModel<T>
            {
                Data = data,
                Ok = false,
                Message = "an error occured",
            };
        }
        public static ResponseModel<T> Error(string message, T data)
        {
            return new ResponseModel<T>
            {
                Data = data,
                Ok = false,
                Message = message
            };
        }
    }
}
