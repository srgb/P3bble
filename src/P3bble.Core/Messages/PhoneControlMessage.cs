﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using P3bble.Core.Constants;

namespace P3bble.Core.Messages
{
    /// <summary>
    /// Represents the phone control type
    /// </summary>
    internal enum PhoneControlType : byte
    {
        /// <summary>
        /// Answer the phone
        /// </summary>
        Answer = 1,

        /// <summary>
        /// Hang up the phone
        /// </summary>
        HangUp = 2,

        /// <summary>
        /// Get state
        /// </summary>
        GetState = 3,

        /// <summary>
        /// Incoming call
        /// </summary>
        IncomingCall = 4,

        /// <summary>
        /// Outgoing call
        /// </summary>
        OutgoingCall = 5,

        /// <summary>
        /// Missed call
        /// </summary>
        MissedCall = 6,

        /// <summary>
        /// Ring the phone
        /// </summary>
        Ring = 7,

        /// <summary>
        /// Start the phone
        /// </summary>
        Start = 8,

        /// <summary>
        /// End the phone
        /// </summary>
        End = 9
    }

    internal class PhoneControlMessage : P3bbleMessage
    {
        private PhoneControlType _type;
        private List<string> _parts;
        private ushort _length;
        private byte[] _cookie;

        public PhoneControlMessage(PhoneControlType type, byte[] cookie, params string[] parts)
            : base(P3bbleEndpoint.PhoneControl)
        {
            this._type = type;
            this._parts = parts.ToList();
            this._length = 0;
            this._cookie = cookie;
        }

        protected override ushort PayloadLength
        {
            get
            {
                return this._length;
            }
        }

        protected override void AddContentToMessage(List<byte> payload)
        {
            string[] parts = this._parts.ToArray();
            byte[] data = { (byte)this._type, };

            data = data.Concat(this._cookie).ToArray();

            foreach (string part in parts)
            {
                byte[] bytePart = Encoding.UTF8.GetBytes(part);
                if (bytePart.Length > 255)
                {
                    bytePart = bytePart.Take(255).ToArray();
                }

                byte[] len = { Convert.ToByte(bytePart.Length) };
                data = data.Concat(len).Concat(bytePart).ToArray();
            }

            this._length = (ushort)data.Length;

            base.AddContentToMessage(payload);
            payload.AddRange(data);
        }

        protected override void GetContentFromMessage(List<byte> payload)
        {
            base.GetContentFromMessage(payload);
        }
    }
}
