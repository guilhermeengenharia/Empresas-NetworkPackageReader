using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPRClient.ValueObject
{
    public class ProtocoloTCP_ISO8583 : IValueObject
    {

        public DateTime Time {get;set;}
        public string IP_Origem {get;set;}
        public string TCP_Origem {get;set;}
        public string IP_Destino {get;set;}
        public string TCP_Destino {get;set;}
            
        public MensagemISO8583 MensagemProcolo { get; set; }       
        public override string ToString()
        {
            StringBuilder concatenacao = new StringBuilder();

            concatenacao.Append(Time.ToString());
            concatenacao.Append("|");
            concatenacao.Append(IP_Origem);
            concatenacao.Append("|");
            concatenacao.Append(TCP_Origem);
            concatenacao.Append("|");
            concatenacao.Append(IP_Destino);
            concatenacao.Append("|");
            concatenacao.Append(TCP_Destino);
            concatenacao.Append("|");
            concatenacao.Append(MensagemProcolo.ToString());

            return concatenacao.ToString();
        }

        public string ToTrasactSQL()
        {

            
            StringBuilder concatenacao = new StringBuilder();

            concatenacao.Append("INSERT INTO public.TB_MONITORAMENTO_GLOBAL_PAYMENTS ");
            concatenacao.Append(Environment.NewLine);
            concatenacao.Append("(ID_MENSAGEM,");
            concatenacao.Append(Environment.NewLine);
            concatenacao.Append("IP_ORIGEM,");
            concatenacao.Append(Environment.NewLine);
            concatenacao.Append("PORTA_ORIGEM,");
            concatenacao.Append(Environment.NewLine);
            concatenacao.Append("ID_DESTINO,");
            concatenacao.Append(Environment.NewLine);
            concatenacao.Append("PORTA_DESTINO,");
            concatenacao.Append(Environment.NewLine);
            concatenacao.Append("DT_MONITORAMENTO,");
            concatenacao.Append(Environment.NewLine);
            concatenacao.Append("DS_MENSAGEM_PROTOCOLO)");
            concatenacao.Append(Environment.NewLine);
            concatenacao.Append("VALUES (");
            concatenacao.Append(Environment.NewLine);
            concatenacao.Append("public.SEQ_MONITORAMENTO_GLOBAL_PAYMENTS.NEXTval,");
            concatenacao.Append(Environment.NewLine);
            concatenacao.Append("'" + IP_Origem + "'");
            concatenacao.Append("," + Environment.NewLine);
            concatenacao.Append("'" + TCP_Origem + "'");
            concatenacao.Append("," + Environment.NewLine);
            concatenacao.Append("'" + IP_Destino + "'");
            concatenacao.Append("," + Environment.NewLine);
            concatenacao.Append("'" + TCP_Destino + "'");
            concatenacao.Append("," + Environment.NewLine);
            concatenacao.Append("'" + Time.ToString("yyyy-MM-dd hh:mm:ss") + "'");
            concatenacao.Append("," + Environment.NewLine);
            concatenacao.Append("'" + MensagemProcolo.ToString().Replace("'","''") + "'");
            concatenacao.Append(");" + Environment.NewLine);

            return concatenacao.ToString();
        }
    }
}
