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
public partial class CResultCfg : TBase
{
  private int _retCode;
  private string _retInfo;
  private double _score;

  public int RetCode
  {
    get
    {
      return _retCode;
    }
    set
    {
      __isset.retCode = true;
      this._retCode = value;
    }
  }

  public string RetInfo
  {
    get
    {
      return _retInfo;
    }
    set
    {
      __isset.retInfo = true;
      this._retInfo = value;
    }
  }

  public double Score
  {
    get
    {
      return _score;
    }
    set
    {
      __isset.score = true;
      this._score = value;
    }
  }


  public Isset __isset;
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public struct Isset {
    public bool retCode;
    public bool retInfo;
    public bool score;
  }

  public CResultCfg() {
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
              RetCode = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              RetInfo = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.Double) {
              Score = iprot.ReadDouble();
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
      TStruct struc = new TStruct("CResultCfg");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.retCode) {
        field.Name = "retCode";
        field.Type = TType.I32;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(RetCode);
        oprot.WriteFieldEnd();
      }
      if (RetInfo != null && __isset.retInfo) {
        field.Name = "retInfo";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(RetInfo);
        oprot.WriteFieldEnd();
      }
      if (__isset.score) {
        field.Name = "score";
        field.Type = TType.Double;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(Score);
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
    StringBuilder __sb = new StringBuilder("CResultCfg(");
    bool __first = true;
    if (__isset.retCode) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("RetCode: ");
      __sb.Append(RetCode);
    }
    if (RetInfo != null && __isset.retInfo) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("RetInfo: ");
      __sb.Append(RetInfo);
    }
    if (__isset.score) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Score: ");
      __sb.Append(Score);
    }
    __sb.Append(")");
    return __sb.ToString();
  }

}

