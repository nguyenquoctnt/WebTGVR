/*!
 *
 * Jquery Mapael - Dynamic maps jQuery plugin (based on raphael.js)
 * Requires jQuery and Mapael
 *
 * Map of World by country
 * 
 * @source http://backspace.com/mapapp/javascript_world/
 */

(function (factory) {
    if (typeof exports === 'object') {
        // CommonJS
        module.exports = factory(require('jquery'), require('mapael'));
    } else if (typeof define === 'function' && define.amd) {
        // AMD. Register as an anonymous module.
        define(['jquery', 'mapael'], factory);
    } else {
        // Browser globals
        factory(jQuery, jQuery.mapael);
    }
}(function ($, Mapael) {

    "use strict";
    
    $.extend(true, Mapael,
        {
            maps :  {
                world_countries : {
                    width : 1000,
                    height : 400,
                    getCoords : function (lat, lon) {
                        var xfactor = 2.752;
                        var xoffset = 473.75;
                        var x = (lon * xfactor) + xoffset;
                        
                        var yfactor = -2.753;
                        var yoffset = 231;
                        var y = (lat * yfactor) + yoffset;
                        
                        return {x : x, y : y};
                    },
                    'elems': {
                        "AE" : "M615.622,164.177l0.582,0.000l0.000,0.580l2.324,-0.289l2.326,0.000l1.455,0.000l2.033,-1.743l2.034,-1.08A1708A88",
                        "AF" : "M642.364,132.815l2.617,1.162l2.034,-0.291l0.581,-1.452l2.325,-0.291l1.454,-0.870l0.583,-2.323l2.326,01675B0AFE",
                        "AL" : "M530.451,115.973l-0.289,0.871l0.289,1.161l1.165,0.581l0.000,0.872l-0.873,0.291l-0.292,0.869l-1.160,10EA7182A97",
                        "AM" : "M593.82,118.005l3.780,-0.580l0.581,0.870l0.871,0.291l-0.289,0.872l1.452,0.871l-0.871,0.871l1.163,0.80E0F7CFA8D",
                        "AO" : "M518.825,247.227l0.582,2.032l0.871,1.453l0.581,0.87l0.874,1.452h2.033l0.873-0.581l1.452,0.581l0.582-0D133EBA25",
                        "AR" : "M293.546,382.836h-2.616l-1.454-0.87h-1.745h-2.907v-6.389l1.163,1.45l1.163,2.033l3.779,1.744l3.778,0.0BFBE97ABB",
                        "AT" : "M520.57,98.549l-0.292,1.162l-1.453,0.000l0.582,0.581l-1.164,1.742l-0.291,0.580l-2.616,0.000l-1.162,00FB516AAF4",
                        "AU" : "M874.039,343.054l2.616,0.871l1.454-0.29l2.325-0.581l1.453,0.291l0.291,3.193l-0.87,0.872l-0.293,2.3210DE4E1DAC0",
                        "AZ" : "M597.6,121.78l0.873,0.581h1.16v0.581l1.163,1.451l-2.033-0.29l-1.163-1.453l-0.582-0.871H597.6zM604.280FB051EAC4",
                        "BA" : "M526.091,107.552l0.871,0.000l-0.581,1.161l1.455,1.162l-0.582,1.161l-0.581,0.291l-0.582,0.290l-0.872,03BC849A1E",
                        "BD" : "M728.989,170.275l-0.292,2.033l-0.871,-0.291l0.291,2.033l-0.872,-1.452l-0.292,-1.452l-0.290,-1.162l-1018AA66AB6",
                        "BE" : "M482.78,89.837l2.034,0.000l2.617,-0.580l1.745,1.451l1.452,0.582l-0.290,1.742l-0.583,0.291l-0.290,1.40311402A89",
                        "BF" : "M465.919,204.54l-1.744,-0.872l-1.452,0.291l-0.872,0.581l-1.164,-0.581l-0.579,-0.871l-1.165,-0.582l-00B6CED3AD3",
                        "BG" : "M536.265,109.294l0.581,1.162l1.164,-0.291l2.035,0.581l4.071,0.000l1.452,-0.581l3.196,-0.581l2.035,0.0BA1D9AA43",
                        "BI" : "M554.579,243.451l-0.290,-3.484l-0.583,-1.161l1.743,0.290l0.874,-1.743l1.454,0.291l0.000,0.871l0.582,0277CB3AF4",
                        "BJ" : "M481.037,213.833l-2.037,0.290l-0.872,-2.033l0.290,-6.388l-0.579,-0.289l-0.291,-1.454l-0.872,-0.871l-0B80526A1B",
                        "BN" : "M787.998,218.479l1.163,-0.872l2.324,-1.741l0.000,1.451l-0.291,1.743l-1.163,0.000l-0.580,0.870l1.453,1.451z",
                        "BO" : "M300.812,291.656l-3.197,-0.290l-1.163,2.323l-1.452,-2.033l-3.781,-0.582l-2.033,2.324l-2.036,0.582l-10383432ACF",
                        "BR" : "M315.056,314.017l3.778,-3.777l3.198,-2.613l1.745,-1.163l2.325,-1.450l0.000,-2.324l-1.454,-1.450l-1.10BA6545A7E",
                        "BS" : "M260.408,165.628h-0.872l-0.581-1.452l-1.163-0.871l0.872-1.743l0.581,0.291l1.164,2.033V165.628zM259.503A3D9EA8C",
                        "BT" : "M726.082,154.594l1.163,0.871l0.000,1.742l-2.326,0.000l-2.326,-0.290l-1.744,0.581l-2.615,-1.162l0.00006E4E97A39",
                        "BW" : "M544.405,281.784l0.582,0.580l0.870,1.742l2.907,2.903l1.455,0.292l0.000,0.870l0.580,1.744l2.326,0.5790E1F930A2D",
                        "BY" : "M538.301,82.579l2.907,0.000l2.908,-0.872l0.578,-1.452l2.326,-1.162l-0.290,-1.160l1.745,-0.291l2.906,05904E1A7F",
                        "BZ" : "M228.433,181.89l0.000,-0.290l0.290,-0.290l0.582,0.580l0.872,-1.742l0.580,0.000l0.000,0.290l0.581,0.001819E8AF1",
                        "CA" : "M298.487,102.905l2.035,0.291h2.617l-1.454,1.162l-0.872,0.29l-3.488-1.162l-0.873-1.161l1.163-0.872L2905475B1A8B",
                        "CD" : "M558.648,221.382l-0.289,3.196l1.162,0.289l-0.873,0.871l-0.871,0.872l-1.163,1.451l-0.581,1.161l-0.29106BB8EBA86",
                        "CF" : "M515.918,210.638l2.325,-0.290l0.293,-0.871l0.579,0.291l0.582,0.580l3.488,-1.162l1.163,-1.161l1.454,-0B81128A27",
                        "CG" : "M509.523,244.033l-0.874,-0.871l-0.870,0.581l-1.163,1.163l-2.325,-2.906l2.035,-1.742l-0.872,-1.743l0.03B7E4BA8A",
                        "CH" : "M500.22,100.292l0.000,0.580l-0.289,0.581l1.162,0.581l1.452,0.000l-0.289,1.162l-1.163,0.290l-2.034,-00144198A0F",
                        "CI" : "M465.919,217.317l-1.162,0.000l-2.034,-0.580l-1.744,0.000l-3.197,0.580l-2.036,0.581l-2.615,1.162l-0.50020B02A08",
                        "CL" : "M284.825,375.578v6.389h2.907h1.745l-0.872,1.162l-2.326,0.87l-1.454-0.291l-1.744-0.289l-1.744-0.582l-02D7A82A75",
                        "CM" : "M509.814,224.578l-0.291,0.000l-1.744,0.289l-1.744,-0.289l-1.163,0.000l-4.652,0.000l0.582,-2.034l-1.101E6E8EA19",
                        "CN" : "M777.533,179.567l-2.325,1.451l-2.326-0.871v-2.323l1.163-1.453l3.196-0.581h1.455l0.581,0.872l-1.163,10FA9256A3F",
                        "CO" : "M266.221,231.256l-1.163,-0.582l-1.163,-0.870l-0.871,0.290l-2.326,-0.290l-0.582,-1.161l-0.580,0.000l-09A1051ACC",
                        "CR" : "M245.292,208.315l-1.453,-0.580l-0.582,-0.582l0.291,-0.580l0.000,-0.581l-0.872,-0.579l-0.872,-0.582l-074B5E5A1E",
                        "CU" : "M247.326,167.081l2.326,0.290l2.326,0.000l2.325,0.871l1.163,1.161l2.616,-0.290l0.873,0.581l2.325,1.740C3D4B0A25",
                        "CY" : "M567.37,134.557l0.000,0.291l-2.909,1.161l-1.162,-0.581l-0.874,-1.161l1.456,0.000l0.580,0.290l0.582,-0E49146A33",
                        "CZ" : "M520.57,97.388l-1.455,-0.581l-1.163,0.000l-2.326,-0.581l-0.871,0.000l-1.453,1.162l-2.035,-0.871l-1.70789F43A0B",
                        "DE" : "M501.093,79.674l0.000,1.161l2.906,0.582l0.000,0.872l2.617,-0.291l1.744,-0.872l2.907,1.163l1.452,0.870E3508DAAF",
                        "DJ" : "M592.368,196.119l0.581,0.871l0.000,1.161l-1.453,0.582l1.161,0.580l-1.161,1.452l-0.583,-0.290l-0.581,067589CA3D",
                        "DK" : "M508.649,77.933l-1.743,2.323l-2.615-1.452l-0.582-1.162l4.07-0.872L508.649,77.933zM503.708,75.609l-0.0662D42A83",
                        "DO" : "M276.396,176.664l0.290,-0.291l2.034,0.000l1.744,0.580l0.872,0.000l0.291,0.872l1.454,0.000l0.000,0.8706F6430A0A",
                        "DZ" : "M506.906,166.5l-9.592,5.226l-7.849,5.227l-4.070,1.453l-2.906,0.000l0.000,-1.742l-1.452,-0.291l-1.4540F71E61AC5",
                        "EC" : "M252.85,240.258l1.453,-2.033l-0.581,-1.162l-1.163,1.162l-1.744,-1.162l0.581,-0.581l-0.291,-2.613l0.8049B6B8A54",
                        "EE" : "M540.626,72.125l0.291,-1.743l-0.872,0.290l-1.744,-0.870l-0.291,-1.743l3.489,-0.581l3.488,-0.582l2.9002B8A13A26",
                        "EG" : "M569.987,149.947l-0.874,0.872l-0.581,2.323l-0.873,1.162l-0.582,0.580l-0.870,-0.871l-1.164,-1.161l-2.012009BA7F",
                        "ER" : "M590.332,196.409l-0.872,-0.871l-1.162,-1.452l-1.163,-1.162l-0.871,-0.870l-2.326,-1.162l-1.744,0.000l030345DABE",
                        "ES" : "M448.769,115.683l0.292,-1.743l-1.163,-1.452l3.778,-1.742l3.489,0.290l3.778,0.000l2.908,0.581l2.324,-03825ADAFA",
                        "ET" : "M578.125,189.73l1.743,1.452l1.453,-0.870l0.873,0.580l1.744,0.000l2.326,1.162l0.871,0.870l1.163,1.16206F40CCAC4",
                        "FI" : "M552.544,41.053l-0.584,2.033l4.363,1.743l-2.617,2.032l3.198,3.194l-1.744,2.324l2.325,2.033l-1.162,1.0ECB829AA1",
                        "FJ" : "M964.732,278.588l0.873,0.871l-0.292,1.452l-1.744,0.291l-1.452-0.291l-0.292-1.162l0.873-0.87l1.455,0.06E50E6ADE",
                        "FK" : "M305.173,373.544l3.488,-1.741l2.326,0.870l1.744,-1.161l2.034,1.161l-0.872,0.871l-3.778,0.872l-1.164,0D18756AE8",
                        "FR" : "M329.008,223.997l-0.873,1.162h-1.453l-0.29-0.581l-0.582-0.292l-0.872,0.873l-1.162-0.581l0.581-1.162l0A45778AC2",
                        "GA" : "M504.291,242l-2.908,-2.904l-1.744,-2.322l-1.744,-2.905l0.291,-0.871l0.582,-0.871l0.581,-2.033l0.582,0F1DF7CA6D",
                        "GB" : "M458.072,80.835l-1.452,2.033l-2.036-0.58h-1.745l0.582-1.453l-0.582-1.451l2.326-0.291L458.072,80.835z09C712AA09",
                        "GE" : "M588.298,116.844l0.291,-1.161l-0.582,-2.034l-1.743,-0.871l-1.455,-0.580l-1.162,-0.581l0.581,-0.581l204D7833A84",
                        "GH" : "M476.676,214.704l-4.361,1.452l-1.452,1.161l-2.617,0.581l-2.327,-0.581l0.000,-1.161l-1.162,-2.323l0.806A5B69A27",
                        "GL" : "M344.996,3.593l9.302,-1.451l9.593,0.000l3.488,-0.871l9.883,-0.291l21.800,0.291l17.442,2.322l-5.232,003A850FA1B",
                        "GM" : "M427.549,194.667l0.291,-1.162l2.909,0.000l0.581,-0.581l0.871,0.000l1.163,0.581l0.873,0.000l0.870,-0.0393302AAC",
                        "GN" : "M450.514,209.768l-0.871,0.000l-0.582,1.161l-0.581,0.000l-0.582,-0.581l0.290,-1.162l-1.162,-1.741l-0.01B9D45A0B",
                        "GQ" : "M499.931,228.061l-0.582,-0.290l0.871,-3.193l4.652,0.000l0.000,3.483l-4.070,0.000l0.871,0.000z",
                        "GR" : "M538.882,132.815l1.744,0.871l2.034-0.29l2.033,0.29v0.582l1.455-0.291l-0.292,0.581l-4.067,0.291v-0.290C2E9E4A1C",
                        "GT" : "M225.816,193.215l-1.453,-0.580l-1.744,0.000l-1.163,-0.581l-1.454,-1.162l0.000,-0.871l0.291,-0.581l-003DB539AEA",
                        "GW" : "M432.201,200.475l-1.452,-1.162l-1.164,0.000l-0.582,-0.871l0.000,-0.291l-0.871,-0.580l-0.292,-0.581l10496FA1ACA",
                        "GY" : "M309.243,208.025l1.744,0.871l1.744,1.742l0.000,1.452l1.162,0.000l1.453,1.452l1.163,0.873l-0.582,2.61095C23FAB5",
                        "HN" : "M233.374,195.248l-0.291,-0.871l-0.872,-0.291l0.000,-1.162l-0.291,-0.289l-0.582,0.000l-1.161,0.289l0.02CC198AE6",
                        "HR" : "M525.51,104.647l0.871,1.163l0.873,0.870l-1.163,0.872l-1.163,-0.581l-2.033,0.000l-2.325,-0.291l-1.16305D437EAE3",
                        "HT" : "M272.326,176.083l1.744,0.290l2.326,0.291l0.290,1.451l-0.290,1.161l-0.582,0.582l0.582,0.580l0.000,0.809CC442AE4",
                        "HU" : "M518.243,102.034l1.164,-1.742l-0.582,-0.581l1.453,0.000l0.292,-1.162l1.454,0.872l0.871,0.290l2.324,-02CB7D5AFB",
                        "ID" : "M806.019,259.132h-1.163l-3.488-2.033l2.326-0.289l1.454,0.58l1.162,0.871L806.019,259.132zM816.193,258021D82FA00",
                        "IE" : "M456.62,82.869l0.579,2.032l-2.034,2.323l-4.942,1.743l-3.779,-0.581l2.036,-2.904l-1.454,-2.613l3.779,01425CBA0A",
                        "IL" : "M572.021,140.946l-0.293,0.870l-1.163,-0.289l-0.578,1.743l0.871,0.289l-0.871,0.581l0.000,0.581l1.160,04D5D11AC7",
                        "IN" : "M688.002,133.396l2.909,3.194l-0.293,2.324l1.163,1.160l0.000,1.453l-2.036,-0.291l0.873,2.904l2.615,1.0C44125AD1",
                        "IQ" : "M598.763,131.943l1.744,0.872l0.289,1.742l-1.452,0.871l-0.581,2.033l2.033,2.613l3.200,1.453l1.454,2.300B18D0A86",
                        "IR" : "M622.309,128.75l2.323,-0.582l2.036,-1.742l1.745,0.291l1.162,-0.581l2.034,0.290l2.907,1.452l2.325,0.20016718A3E",
                        "IS" : "M433.944,48.313l-0.870,1.742l3.196,1.742l-3.488,2.033l-8.138,2.033l-2.326,0.581l-3.488,-0.581l-7.8490C739D5AD0",
                        "IT" : "M516.5,125.846l-0.873,2.033l0.292,0.872l-0.582,1.451l-2.034-0.871l-1.454-0.291l-3.777-1.451l0.289-1.0280C30A16",
                        "JM" : "M260.116,180.148l2.036,0.290l1.452,0.581l0.291,0.871l-1.743,0.000l-0.872,0.291l-1.454,-0.291l-1.744,0728E07AF6",
                        "JO" : "M571.728,141.816l0.293,-0.870l3.195,1.161l5.234,-2.903l1.163,3.193l-0.582,0.582l-5.522,1.451l2.905,20D1B682A34",
                        "JP" : "M844.39,137.17l0.289,0.871l-1.452,1.742l-1.163-1.161l-1.454,0.871l-0.58,1.452l-2.035-0.581l0.292-1.403146CCA37",
                        "KE" : "M586.553,233.289l1.745,2.323l-2.034,1.162l-0.582,1.161l-1.163,0.000l-0.291,2.032l-0.872,1.161l-0.5810C49511A04",
                        "KG" : "M669.108,114.811l0.581,-1.162l1.745,-0.580l4.649,0.871l0.292,-1.452l1.745,-0.581l3.779,1.162l1.161,-09828DAAC2",
                        "KH" : "M758.638,201.637l-1.162,-1.453l-1.454,-2.613l-0.580,-3.485l1.741,-2.323l3.781,-0.581l2.326,0.581l2.30A480B0AF0",
                        "KO" : "M531.032,115.392l-0.289,0.581l-0.292,0.000l-0.289,-1.162l-0.582,-0.290l-0.581,-0.581l0.581,-0.871l0.01325FBA55",
                        "KP" : "M833.343,114.229l0.292,0.582l-0.872,0.000l-1.164,0.872l-0.872,0.870l0.000,2.033l-1.452,0.581l-0.291,07A4C2DA90",
                        "KR" : "M826.948,124.684l2.617,3.194l0.582,2.034l0.000,2.903l-1.163,1.742l-2.326,0.582l-2.325,0.870l-2.326,002C8D3DA9A",
                        "KW" : "M605.74,148.496l0.581,1.162l-0.291,0.580l0.871,2.323l-1.743,0.000l-0.873,-1.452l-2.326,-0.290l2.037,-2.614l-1.744,-0.291z",
                        "KZ" : "M669.108,114.811l-1.454,0.291l-3.779,2.033l-1.163,2.032l-1.163,0.000l-0.580,-1.452l-3.489,0.000l-0.5002E27FA9E",
                        "LA" : "M763.29,191.763l0.872,-1.451l0.291,-2.323l-2.327,-2.324l0.000,-2.613l-2.325,-2.323l-2.036,0.000l-0.500C0C35A3A",
                        "LB" : "M572.31,139.494l-0.582,0.000l-0.291,0.580l-0.872,0.000l0.872,-2.323l1.454,-2.032l1.163,0.000l0.581,10A677D6AB6",
                        "LK" : "M699.047,210.348l-0.579,2.904l-1.165,0.581l-2.323,0.582l-1.455,-2.034l-0.292,-4.066l1.166,-4.356l2.00ACD41DA76",
                        "LR" : "M452.549,219.06l-0.873,0.000l-2.615,-1.453l-2.617,-2.032l-2.324,-1.452l-1.744,-1.742l0.580,-0.872l0.0E2E621A18",
                        "LS" : "M553.416,310.531l1.163,0.872l-0.873,1.451l-0.581,0.871l-1.454,0.292l-0.581,0.869l-0.871,0.291l-2.0360769ED5AE7",
                        "LT" : "M536.265,81.417l-0.291,-0.582l0.582,-0.870l-1.454,-0.291l-2.906,-0.581l-0.580,-2.322l3.197,-0.871l4.07B2522A9B",
                        "LU" : "M490.338,93.032l0.579,0.581l0.000,1.452l-0.871,0.000l-0.581,-0.291l0.290,-1.451l-0.583,0.291z",
                        "LV" : "M531.616,76.771l0.290,-2.033l1.162,-1.742l2.616,-0.871l2.326,2.033l2.035,0.000l0.581,-2.033l2.325,-00A1A6A4A29",
                        "LY" : "M514.755,167.951l-2.036,1.162l-1.452,-1.452l-4.361,-1.161l-1.452,-1.743l-2.035,-1.451l-1.163,0.580l-005F9A4AC6",
                        "MA" : "M459.526,132.525l1.743,1.161l2.616,0.000l2.615,0.581l1.164,0.000l1.163,1.742l0.291,1.742l0.871,2.9050D89103AD4",
                        "MD" : "M547.02,98.259l0.584,-0.290l2.033,-0.290l2.034,0.870l1.162,0.000l1.166,0.872l-0.293,0.871l1.164,0.58018BC69A7C",
                        "ME" : "M528.417,113.94l-0.292,-0.291l-1.163,1.162l0.000,0.872l-0.581,0.000l-0.579,-0.872l-1.163,-0.582l0.28054D821A62",
                        "MG" : "M610.099,265.23l0.873,1.163l0.582,1.742l0.579,3.485l0.584,1.160l-0.293,1.454l-0.581,0.579l-0.871,-1.06E325DAC8",
                        "MK" : "M530.451,115.973l0.292,0.000l0.289,-0.581l1.455,-0.581l0.581,0.000l1.161,-0.290l1.165,0.000l1.452,0.0D3E34EAC8",
                        "ML" : "M440.34,190.602l0.871,-0.290l0.583,-1.743l0.872,0.000l1.744,0.871l1.455,-0.580l1.161,0.000l0.581,-0.0D70AD3A0A",
                        "MM" : "M747.882,175.501l-1.743,1.163l-2.034,0.000l-1.163,3.194l-1.165,0.290l1.455,2.613l1.744,1.742l1.163,20A26DC6A08",
                        "MN" : "M715.327,95.356l2.907,-0.582l5.232,-2.323l4.069,-1.161l2.616,0.871l2.908,0.000l1.741,1.162l2.617,0.20B1F9A1A0C",
                        "MR" : "M440.34,190.602l-2.034,-1.742l-1.454,-2.033l-2.034,-0.580l-1.163,-0.872l-1.454,0.000l-1.452,0.581l-101AB48CAF5",
                        "MW" : "M568.822,262.618l-0.582,2.032l0.582,3.776l1.165,-0.291l0.871,0.871l1.163,2.034l0.289,3.483l-1.163,0.0C9E706AFE",
                        "MX" : "M206.341,159.82l-1.163,2.324l-0.291,2.033l-0.290,3.774l-0.291,1.162l0.581,1.743l0.872,1.161l0.582,2.00F1F9DAAF",
                        "MY" : "M751.953,213.833l0.29,1.451l1.744-0.289l0.873-1.162l0.582,0.29l1.741,1.743l1.165,1.741l0.29,1.743l-00DC9BF9A2C",
                        "MZ" : "M568.822,262.618l2.036,-0.292l3.486,0.872l0.581,-0.291l2.036,-0.289l0.872,-0.581l1.746,0.000l2.907,-0E080EEAE7",
                        "NA" : "M518.825,309.661l-2.036,-2.325l-1.163,-2.033l-0.579,-2.613l-0.584,-2.032l-1.161,-4.065l0.000,-3.485l07C26AAA77",
                        "NC" : "M930.142,289.042l2.325,1.452l1.452,1.454l-1.162,0.579l-1.453,-0.871l-2.036,-1.162l-1.744,-1.452l-1.70C8E9D1AA5",
                        "NCY" : "M563.881,134.267l0.289,0.000l0.291,-0.581l2.035,0.000l2.326,-0.871l-1.745,1.162l0.293,0.580l-0.293,0042C33FA71",
                        "NE" : "M479.583,198.151l0.291,-2.032l-3.198,-0.581l-0.291,-1.161l-1.453,-2.033l-0.292,-1.162l0.292,-1.161l1008887FAEF",
                        "NG" : "M497.023,217.898l-2.615,0.871l-1.164,0.000l-1.161,0.581l-2.037,0.000l-1.452,-1.743l-0.873,-2.032l-2.08530D5AD2",
                        "NI" : "M237.734,200.475l-0.872,-0.871l-1.163,-1.162l-0.581,-0.871l-1.163,-0.871l-1.454,-1.162l0.291,-0.580l0C4BFD2AFB",
                        "NL" : "M490.628,83.74l2.035,0.000l0.581,1.161l-0.581,2.323l-0.871,1.162l-1.454,0.000l0.290,2.904l-1.452,-0.0F68D2FAAC",
                        "NO" : "M551.381,35.246l8.43,2.032l-3.488,0.582l3.198,1.742l-4.942,1.161l-2.034,0.29l1.161-2.032l-3.486-1.1607A5AF5AAE",
                        "NP" : "M716.198,154.304l0.000,1.161l0.291,1.742l-0.291,1.162l-2.326,0.000l-3.197,-0.581l-2.325,-0.290l-1.450912D15AAD",
                        "NZ" : "M949.907,343.345l0.873,1.161l1.745-1.161l0.87,1.161v1.161l-0.87,1.452l-2.036,2.033l-1.163,1.161l0.870A765B1AB4",
                        "OM" : "M635.678,172.888l-0.871,1.742h-1.163l-0.58,0.581l-0.582,1.452l0.29,1.742l-0.29,0.581l-1.163-0.29l-1.0FC95F5AFD",
                        "PA" : "M259.244,211.219l-0.872,-0.871l-0.580,-1.452l0.872,-0.871l-0.872,-0.290l-0.582,-0.871l-1.163,-0.581l05CF870A29",
                        "PE" : "M282.208,279.17l-0.872,1.451l-1.163,0.872l-2.905,-1.743l-0.292,-1.162l-5.232,-2.613l-4.942,-3.195l-20AB9FABAC8",
                        "PG" : "M902.817,249.55l-0.873,0.29l-1.163-0.871l-1.162-1.742l-0.581-2.032l0.581-0.289l0.29,0.579l0.583,0.8708E5320AD5",
                        "PH" : "M821.715,207.735l0.292,2.033v1.451l-0.872,2.322l-0.871-2.612l-1.454,1.452l0.871,2.033l-0.871,1.16l-300F4104A9F",
                        "PK" : "M680.735,128.75l2.036,1.451l0.870,2.033l4.361,1.162l-2.617,2.323l-3.196,0.290l-4.069,-0.581l-1.453,10079B36A28",
                        "PL" : "M515.047,90.418l-1.165,-1.742l0.292,-0.870l-0.581,-1.452l-1.163,-1.162l0.872,-0.581l-0.583,-1.452l1.092585DABC",
                        "PR" : "M291.219,180.148l1.455,0.000l0.581,0.581l-0.872,0.871l-2.035,0.000l-1.453,0.000l-0.291,-1.162l0.582,-0.290l-2.033,0.000z",
                        "PS" : "M571.728,141.816l0.000,1.743l-0.581,0.871l-1.160,0.291l0.000,-0.581l0.871,-0.581l-0.871,-0.289l0.578,-1.743l-1.163,-0.289z",
                        "PT" : "M448.769,115.683l1.163,-0.581l1.163,-0.291l0.581,1.162l1.744,0.000l0.291,-0.290l1.746,0.000l0.581,1.0A419D7AF0",
                        "PY" : "M301.103,292.237l1.163,-3.485l0.000,-1.451l1.453,-2.324l4.652,-0.871l2.616,0.000l2.615,1.451l0.000,00E6DC61AD0",
                        "QA" : "M613.587,162.725l0.000,-1.743l0.582,-1.452l0.873,-0.290l0.871,0.871l0.000,1.451l-0.581,1.744l-0.872,0.000l0.873,0.581z",
                        "RO" : "M536.265,99.13l1.163,-0.581l1.744,0.581l1.745,0.000l1.163,0.581l1.162,-0.581l2.036,-0.291l0.579,-0.50BD8A86A58",
                        "RS" : "M531.325,106.1l1.454,0.580l0.289,1.161l1.745,0.872l0.871,-0.580l0.581,0.289l-0.581,0.291l0.581,0.5810CD57E1AF9",
                        "RU" : "M869.098,91.29l2.907,4.936l-4.07-0.87l-1.743,4.065l2.614,2.613v2.033l-2.034-1.743l-1.743,2.323l-0.58053BD7CAB4",
                        "RW" : "M557.485,234.16l1.163,1.452l-0.289,1.741l-0.582,0.291l-1.454,-0.291l-0.874,1.743l-1.743,-0.290l0.2930D2FC8BA40",
                        "SA" : "M591.496,185.956l-0.291,-1.162l-0.873,-0.871l-0.290,-1.162l-1.453,-0.871l-1.454,-2.323l-0.582,-2.32205D58ADABA",
                        "SB" : "M919.968,259.712l0.871,0.873h-2.034l-0.873-1.453l1.452,0.58H919.968zM916.48,257.972l-0.874,0.289l-1.0A20A12A53",
                        "SD" : "M567.37,204.831l-0.582,0.000l0.000,-1.452l-0.292,-0.873l-1.453,-1.160l-0.292,-1.742l0.292,-2.033l-1.0339FD1A74",
                        "SE" : "M534.813,50.346l-2.617,1.742l0.291,1.742l-4.362,2.324l-5.230,2.323l-2.036,3.774l2.036,2.033l2.615,1.060D372ABC",
                        "SI" : "M511.848,102.905l2.326,0.291l1.162,-0.582l2.616,0.000l0.291,-0.580l0.582,0.000l0.582,1.162l-2.325,0.0901F45A72",
                        "SK" : "M525.802,94.774l0.000,0.291l1.160,-0.582l1.455,1.163l1.453,-0.581l1.455,0.291l2.033,-0.582l2.616,1.10DE7A64A82",
                        "SL" : "M442.376,212.381l-0.873,-0.291l-2.034,-1.161l-1.455,-1.452l-0.289,-0.871l-0.292,-2.033l1.455,-1.451l0CB163EA6E",
                        "SN" : "M427.84,193.505l-1.162,-2.032l-1.454,-1.161l1.164,-0.291l1.452,-2.032l0.582,-1.452l0.871,-0.872l1.4509F3E4AAD1",
                        "SO" : "M610.681,199.023l1.452,-0.290l1.162,-0.871l1.165,0.000l0.000,0.871l-0.291,1.451l0.000,1.453l-0.582,10C12301A1D",
                        "SOL" : "M608.355,204.831l-1.163,1.742l-1.742,2.323l-2.328,0.000l-9.010,-3.194l-1.163,-0.871l-0.873,-1.452l-10288060ABD",
                        "SR" : "M316.509,214.415l3.198,0.580l0.290,-0.291l2.326,-0.289l2.907,0.580l-1.453,2.612l0.289,1.743l1.164,1.053E6E7A13",
                        "SS" : "M567.37,204.831l0.000,2.322l-0.582,0.872l-1.455,0.000l-0.872,1.452l1.746,0.291l1.452,1.161l0.290,1.10862243AD3",
                        "SV" : "M232.211,194.086l-0.291,0.581l-1.743,0.000l-0.872,-0.290l-1.164,-0.581l-1.452,0.000l-0.873,-0.581l0.0DAC9DDAFB",
                        "SY" : "M580.45,139.204l-5.234,2.903l-3.195,-1.161l0.289,-0.290l0.000,-1.162l0.873,-1.452l1.452,-1.161l-0.580858A0BA65",
                        "SZ" : "M562.136,304.433l-0.581,1.161l-1.744,0.290l-1.452,-1.451l-0.292,-0.871l0.870,-1.161l0.293,-0.581l0.80E8F216A2F",
                        "TD" : "M513.593,195.538l0.289,-1.161l-1.742,-0.291l0.000,-1.742l-1.165,-0.871l1.165,-3.775l3.486,-2.614l0.2076EBFAA53",
                        "TF" : "M663.583,364.542l1.746,0.872l2.617,0.581l0.000,0.291l-0.584,1.452l-4.360,0.000l0.000,-1.452l0.292,-1.161l-0.289,0.583z",
                        "TG" : "M479,214.123l-2.324,0.581l-0.582,-1.162l-0.872,-1.742l0.000,-1.162l0.581,-2.613l-0.871,-0.872l-0.2920D7E0CFA4C",
                        "TH" : "M756.022,197.571l-2.325,-1.452l-2.325,0.000l0.290,-2.033l-2.326,0.000l-0.291,2.904l-1.454,4.065l-0.80F100A7A67",
                        "TJ" : "M669.108,120.329l-0.873,0.871l-2.906,-0.582l-0.291,1.743l2.908,-0.290l3.488,0.871l5.233,-0.291l0.579060F6DEA00",
                        "TL" : "M817.647,255.359l0.580,-0.582l2.327,-0.580l1.744,-0.291l0.871,-0.290l1.164,0.290l-1.164,0.871l-2.90506CF8EFAD0",
                        "TM" : "M642.364,132.815l-0.289,-2.323l-2.034,0.000l-3.200,-2.324l-2.325,-0.290l-2.907,-1.452l-2.034,-0.290l02548B0AF4",
                        "TN" : "M499.931,147.625l-1.163,-4.936l-1.745,-1.162l0.000,-0.581l-2.325,-1.742l-0.290,-2.034l1.745,-1.451l00EADA55ACF",
                        "TR" : "M575.509,117.135l3.777,1.161l3.199-0.291l2.323,0.291l3.489-1.451l2.906-0.291l2.615,1.452l0.292,0.8720E5E9C6ABA",
                        "TT" : "M304.01,201.346l1.454,-0.291l0.581,0.000l0.000,2.033l-2.326,0.291l-0.581,-0.291l0.872,-0.582l0.000,1.160z",
                        "TW" : "M808.926,163.886l-1.744,4.356l-1.163,2.322l-1.452,-2.322l-0.292,-2.033l1.744,-2.614l2.325,-2.322l1.163,0.871l0.581,-1.742z",
                        "TZ" : "M567.077,233.58l0.582,0.289l9.883,5.517l0.291,1.742l3.780,2.615l-1.163,3.484l0.000,1.452l1.744,1.1610875135A42",
                        "UA" : "M561.265,87.806l1.160,0.000l0.584,-0.582l0.872,0.000l2.907,-0.290l1.744,1.742l-0.873,0.581l0.290,0.808DC67BAEF",
                        "UG" : "M561.555,233.869l-3.196,0.000l-0.874,0.291l-1.743,0.871l-0.582,-0.290l0.000,-2.324l0.582,-0.871l0.2903BB9CAA05",
                        "US" : "M45.593,178.406l-0.292,0.581l-0.873-0.581l0.292-0.581l-0.582-1.162l0.29-0.291l0.292-0.29l-0.292-0.580E92AC1A2B",
                        "UY" : "M315.056,314.017l1.744,-0.292l2.907,2.033l0.872,0.000l2.907,1.743l2.325,1.451l1.454,2.032l-1.163,1.108C74DBADA",
                        "UZ" : "M656.899,128.168l0.000,-1.742l-3.487,-1.160l-2.909,-1.162l-2.034,-1.453l-2.906,-1.742l-1.455,-2.904l099A7E5A02",
                        "VE" : "M277.558,198.442l-0.290,0.871l-1.454,0.291l0.872,1.161l0.000,1.452l-1.454,1.451l1.164,2.324l1.162,-001B412BAD6",
                        "VN" : "M771.137,171.726l-3.488,2.324l-2.326,2.614l-0.581,1.742l2.034,2.904l2.617,3.774l2.325,1.743l1.744,2.05A6627AD6",
                        "VU" : "M935.666,276.266l-0.872,0.291l-0.874-1.163v-0.871L935.666,276.266zM933.628,271.91l0.583,2.324l-0.8740BC5BC9A2D",
                        "WS" : "M449.643,156.336l-0.292,-1.452l0.581,0.000l0.000,0.290l0.000,0.291l0.000,4.355l-9.011,-0.290l0.000,70B53AD4A8A",
                        "YE" : "M619.983,185.084l-2.034,0.872l-0.583,1.161l0.000,0.872l-2.616,1.160l-4.651,1.453l-2.326,1.742l-1.1620FA6C4CAFC",
                        "ZA" : "M560.392,311.403l-0.29,0.291l-1.165,1.451l-0.87,1.451l-1.453,2.034l-3.198,2.902l-2.034,1.451l-2.036,0DA4D0FA26",
                        "ZM" : "M563.881,256.229l1.452,1.452l0.582,2.322l-0.582,0.582l-0.290,2.322l0.290,2.323l-0.872,0.873l-0.580,2058A4C1A6F",
                        "ZW" : "M559.521,292.237l-1.454,-0.289l-0.871,0.289l-1.163,-0.581l-1.163,0.000l-1.745,-1.162l-2.326,-0.579l-0F0DFEDAAC"
    
                    }
                }
            }
        }
    );

    return Mapael;

}));