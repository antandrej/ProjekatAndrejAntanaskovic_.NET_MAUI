using ProjekatAndrejAntanaskovic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ProjekatAndrejAntanaskovic.Services
{
    public class MockDataService
    {
        private readonly List<Usluga> uslugaList;
        private readonly List<Termin> terminList;
        private readonly RadnoVreme radnoVreme;

        public MockDataService()
        {
            uslugaList = new List<Usluga>
            { 
                new Usluga { Naziv = "Musko Sisanje", TrajanjeMinuti = 30, Cena = 1200, Opis = "Klasično ili moderno šišanje makazama i mašinicom." },
                new Usluga { Naziv = "Pranje i stilizovanje", TrajanjeMinuti = 30, Cena = 850, Opis = "Pranje kose uz masažu glave i oblikovanje frizure." },
                new Usluga { Naziv = "Uredjivanje brade", TrajanjeMinuti = 30, Cena = 900, Opis = "Oblikovanje brade trimerom i brijanje kontura." },
                new Usluga { Naziv = "Komplet (Sisanje + Brada)", TrajanjeMinuti = 60, Cena = 1800, Opis = "Puni tretman šišanja i kompletno sređivanje brade." }
            };

            terminList = new List<Termin>
            {
                new Termin
                {
                    UslugaId = uslugaList[0].Id,
                    NazivUsluge = uslugaList[0].Naziv,
                    ImeKlijenta = "Pera Petrovic",
                    TelefonKlijenta = "0641234567",
                    UredjajId = "test-device-id",
                    DatumVreme = DateTime.Today.AddHours(10),
                    Status = StatusTermina.Zakazan
                },
                new Termin
                {
                    UslugaId = uslugaList[2].Id,
                    NazivUsluge = uslugaList[2].Naziv,
                    ImeKlijenta = "Marko Jovanovic",
                    TelefonKlijenta = "0600789456",
                    UredjajId = "test-device-id2",
                    DatumVreme = DateTime.Today.AddHours(11),
                    Status = StatusTermina.Zakazan
                }
            };
            radnoVreme = new RadnoVreme();
        }
        public Task<List<Usluga>> GetUslugeAsync()
        {
            return Task.FromResult(uslugaList);
        }

        public Task<List<Termin>> GetTerminZaDatumAsync(DateTime datum)
        {
            var zauzeti = terminList.Where(t => t.DatumVreme.Date == datum.Date && t.Status == StatusTermina.Zakazan).ToList();
            return Task.FromResult(zauzeti);
        }

        public Task<bool> ZakaziTerminAsync(Termin noviTermin)
        {
            terminList.Add(noviTermin);
            return Task.FromResult(true);
        }

        public Task<List<Termin>> GetIstorijuZaUredjajAsync(string uredjajId)
        {
            var istorija = terminList.Where(t => t.UredjajId == uredjajId).OrderByDescending(t => t.DatumVreme).ToList();
            return Task.FromResult(istorija);
        }

        public Task<List<Termin>> SviTerminiAsync()
        {
            return Task.FromResult(terminList.OrderBy(t => t.DatumVreme).ToList());
        }

        public async Task<List<DateTime>> GetSlobodniSlotoviAsync(DateTime datum, int trajanjeMinuti)
        {
            var zauzetiTermini = await GetTerminZaDatumAsync(datum);
            var slobodniSlotovi = new List<DateTime>();

            DateTime pocetak = datum.Date.Add(radnoVreme.PocetakRadnogVremena);
            DateTime kraj = datum.Date.Add(radnoVreme.KrajRadnogVremena);

            DateTime trenutniSlot = pocetak;

            while(trenutniSlot.AddMinutes(trajanjeMinuti) <= kraj)
            {
                bool jeZauzet = zauzetiTermini.Any(t => trenutniSlot < t.DatumVreme.AddMinutes(30) && trenutniSlot.AddMinutes(trajanjeMinuti) > t.DatumVreme);

                if (!jeZauzet && trenutniSlot > DateTime.Now)
                {
                    slobodniSlotovi.Add(trenutniSlot);
                }

                trenutniSlot = trenutniSlot.AddMinutes(radnoVreme.IntervalMinuti);
            }

            return slobodniSlotovi;
        }
    }
}