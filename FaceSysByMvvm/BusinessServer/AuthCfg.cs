/**
 * Autogenerated by Thrift Compiler (0.9.3)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using System.Runtime.Serialization;
using Thrift.Protocol;
using Thrift.Transport;


#if !SILVERLIGHT
[Serializable]
#endif
public partial class AuthCfg : TBase
{
  private int _rtype;
  private int _pid;
  private int _flag;

  public int Rtype
  {
    get
    {
      return _rtype;
    }
    set
    {
      __isset.rtype = true;
      this._rtype = value;
    }
  }

  public int Pid
  {
    get
    {
      return _pid;
    }
    set
    {
      __isset.pid = true;
      this._pid = value;
    }
  }

  public int Flag
  {
    get
    {
      return _flag;
    }
    set
    {
      __isset.flag = true;
      this._flag = value;
    }
  }


  public Isset __isset;
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public struct Isset {
    public bool rtype;
    public bool pid;
    public bool flag;
  }

  public AuthCfg() {
  }

  public void Read (TProtocol iprot)
  {
    iprot.IncrementRecursionDepth();
    try
    {
      TField field;
      iprot.ReadStructBegin();
      while (true)
      {
        field = iprot.ReadFieldBegin();
        if (field.Type == TType.Stop) { 
          break;
        }
        switch (field.ID)
        {
          case 1:
            if (field.Type == TType.I32) {
              Rtype = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.I32) {
              Pid = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I32) {
              Flag = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          default: 
            TProtocolUtil.Skip(iprot, field.Type);
            break;
        }
        iprot.ReadFieldEnd();
      }
      iprot.ReadStructEnd();
    }
    finally
    {
      iprot.DecrementRecursionDepth();
    }
  }

  public void Write(TProtocol oprot) {
    oprot.IncrementRecursionDepth();
    try
    {
      TStruct struc = new TStruct("AuthCfg");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.rtype) {
        field.Name = "rtype";
        field.Type = TType.I32;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Rtype);
        oprot.WriteFieldEnd();
      }
      if (__isset.pid) {
        field.Name = "pid";
        field.Type = TType.I32;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Pid);
        oprot.WriteFieldEnd();
      }
      if (__isset.flag) {
        field.Name = "flag";
        field.Type = TType.I32;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Flag);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }
    finally
    {
      oprot.DecrementRecursionDepth();
    }
  }

  public override string ToString() {
    StringBuilder __sb = new StringBuilder("AuthCfg(");
    bool __first = true;
    if (__isset.rtype) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Rtype: ");
      __sb.Append(Rtype);
    }
    if (__isset.pid) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Pid: ");
      __sb.Append(Pid);
    }
    if (__isset.flag) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Flag: ");
      __sb.Append(Flag);
    }
    __sb.Append(")");
    return __sb.ToString();
  }

}

