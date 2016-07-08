using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPRClient.ENUN
{

    public enum TipoMonitoramento
    {
        MonitorarTCP
    }

    public enum TipoRepositorio
    {
        ArquivoTexto,
        Vertica,
        StreamBase
    }

    public enum TipoValueObject
    {
        ProtocoloTCP_ISO8583,
        Mensagem_ISO8583,
        ItemMensagem_ISO8583
    }

    public enum TipoDevice
    {
        DeviseOffLine_ISO8583,
        DeviceOnLine_ISO8583
    }

    public enum TipoConversor
    {
        Conversor_ISO8583        
    }

    public enum TipoAtributoIso8583
    {
        MensageType,
        BitMap2,
        Primary_Account_Number_PAN,
        Processing_Code,
        Amount_Transaction,
        Amount_Settlement,
        Amount_Cardholder_Billing,
        Transmission_Date_Time,
        Amount_Cardholder_Billing_Fee,
        Conversion_Rate_Settlement,
        Conversion_Rate_Cardholder_Billing,
        Systems_Trace_Audit_Number,
        Time_Local_Transaction,
        Date_Local_Transaction,
        Date_Expiration,
        Date_Settlement,
        Date_Conversion,
        Date_Capture,
        Merchant_Type,
        Acquiring_Institution_Country_Code,
        PAextended_Country_Code,
        Forwarding_Institution_Country_Code,
        Point_Of_Service_Entry_Mode,
        Application_PAnumber,
        Function_Code_Network_International_Identifier_NII,
        Point_Of_Service_Condition_Code,
        Point_Of_Service_Capture_Code,
        Authorizing_Identification_Response_Length,
        Amount_Transaction_Fee,
        Amount_Settlement_Fee,
        Amount_Transaction_Processing_Fee,
        Amount_Settlement_Processing_Fee,
        Acquiring_Institution_Identification_Code,
        Forwarding_Institution_Identification_Code,
        Primary_Account_Number_Extended,
        Track_2_Data,
        Track_3_Data,
        Retrieval_Reference_Number,
        Authorization_Identification_Response,
        Response_Code,
        Service_Restriction_Code,
        Card_Acceptor_Terminal_Identification,
        Card_Acceptor_Identification_Code,
        Card_Acceptor_Name_Location,
        Additional_Response_Data,
        Track_1_Data,
        Additional_Data_ISO,
        Additional_Data_National,
        Additional_Data_Private,
        Currency_Code_Transaction,
        Currency_Code_Settlement,
        Currency_Code_Cardholder_Billing,
        Personal_Identificatio_Number_Data,
        Security_Related_Control_Information,
        Additional_Amounts,
        Reserved_ISO_P1,
        Reserved_ISO_P2,
        Reserved_National_P1,
        Reserved_National_P2,
        Reserved_For_National_Use,
        Advice_Reasocode_Private_Reserved,
        Reserved_Private_P1,
        Reserved_Private_P2,
        Reserved_Private_P3,
        Message_Authentication_Code_MAC,
        BitMap_Binario,

    }

   
}
