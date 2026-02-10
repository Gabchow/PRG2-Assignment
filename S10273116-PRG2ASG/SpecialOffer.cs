//==========================================================
// Student Number : S10273116H
// Student Name : Gabriel Chow
// Partner Name : V Raghav Raj
//==========================================================

using System;

public class SpecialOffer
{
    private string OfferCode;
    private string OfferDesc;
    private double Discount;

    public string offerCode
    {
        get { return OfferCode; }
        set { OfferCode = value; }
    }

    public string offerDesc
    {
        get { return OfferDesc; }
        set { OfferDesc = value; }
    }

    public double discount
    {
        get { return Discount; }
        set { Discount = value; }
    }

    public SpecialOffer() { }

    public SpecialOffer(string offerCode, string offerDesc, double discount)
    {
        OfferCode = offerCode;
        OfferDesc = offerDesc;
        Discount = discount;
    }

    public override string ToString()
    {
        if (Discount > 0)
        {
            return $"{OfferCode}: {OfferDesc} - {Discount}% off";
        }
        else
        {
            return $"{OfferCode}: {OfferDesc}";
        }
    }
}
