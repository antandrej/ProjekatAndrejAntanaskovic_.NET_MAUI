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
                new Usluga { Id = "USL001", Naziv = "Musko Sisanje", TrajanjeMinuti = 30, Cena = 1200, Opis = "Klasično ili moderno šišanje makazama i mašinicom." },
                new Usluga { Id = "USL002", Naziv = "Pranje i stilizovanje", TrajanjeMinuti = 30, Cena = 850, Opis = "Pranje kose uz masažu glave i oblikovanje frizure." },
                new Usluga { Id = "USL003", Naziv = "Uredjivanje brade", TrajanjeMinuti = 30, Cena = 900, Opis = "Oblikovanje brade trimerom i brijanje kontura." },
                new Usluga { Id = "USL004", Naziv = "Komplet (Sisanje + Brada)", TrajanjeMinuti = 60, Cena = 1800, Opis = "Puni tretman šišanja i kompletno sređivanje brade." }
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
                    Status = StatusTermina.Zakazan,
                    TrajanjeMinuti = uslugaList[0].TrajanjeMinuti
                },
                new Termin
                {
                    UslugaId = uslugaList[2].Id,
                    NazivUsluge = uslugaList[2].Naziv,
                    ImeKlijenta = "Marko Jovanovic",
                    TelefonKlijenta = "0600789456",
                    UredjajId = "test-device-id2",
                    DatumVreme = DateTime.Today.AddHours(11),
                    Status = StatusTermina.Zakazan,
                    TrajanjeMinuti = uslugaList[2].TrajanjeMinuti
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
            var zauzeti = terminList.Where(t =>
                t.DatumVreme.Date == datum.Date &&
                (
                    t.Status == StatusTermina.Zakazan ||
                    t.Status == StatusTermina.Potvrdjen
                ))
                .ToList();

            return Task.FromResult(zauzeti);
        }


        public Task<bool> ZakaziTerminAsync(Termin noviTermin)
        {
            bool postojiPreklapanje = terminList.Any(t =>
                t.Status != StatusTermina.Otkazan &&
                noviTermin.DatumVreme <
                    t.DatumVreme.AddMinutes(t.TrajanjeMinuti) &&
                noviTermin.DatumVreme.AddMinutes(noviTermin.TrajanjeMinuti) >
                    t.DatumVreme);

            if (postojiPreklapanje)
            {
                return Task.FromResult(false);
            }

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
                bool jeZauzet = zauzetiTermini.Any(t => trenutniSlot < t.DatumVreme.AddMinutes(t.TrajanjeMinuti) && trenutniSlot.AddMinutes(trajanjeMinuti) > t.DatumVreme);

                if (!jeZauzet && trenutniSlot > DateTime.Now)
                {
                    slobodniSlotovi.Add(trenutniSlot);
                }

                trenutniSlot = trenutniSlot.AddMinutes(radnoVreme.IntervalMinuti);
            }

            return slobodniSlotovi;
        }

        public Task PromeniStatusAsync(string terminId, StatusTermina noviStatus)
        {
            var termin = terminList
                .FirstOrDefault(x => x.Id == terminId);

            if (termin != null)
            {
                termin.Status = noviStatus;
            }

            return Task.CompletedTask;
        }

        public Task DodajUsluguAsync(Usluga usluga)
        {
            uslugaList.Add(usluga);

            return Task.CompletedTask;
        }

        public Task ObrisiUsluguAsync(string id)
        {
            var usluga =
                uslugaList.FirstOrDefault(x => x.Id == id);

            if (usluga != null)
            {
                uslugaList.Remove(usluga);
            }

            return Task.CompletedTask;
        }

        public Task IzmeniUsluguAsync(Usluga nova)
        {
            var postojeca =
                uslugaList.FirstOrDefault(x => x.Id == nova.Id);

            if (postojeca != null)
            {
                postojeca.Naziv = nova.Naziv;
                postojeca.Cena = nova.Cena;
                postojeca.Opis = nova.Opis;
                postojeca.TrajanjeMinuti = nova.TrajanjeMinuti;
            }

            return Task.CompletedTask;
        }

        public Task<List<Usluga>> GetSveUslugeAsync()
        {
            return Task.FromResult(uslugaList.ToList());
        }

        public Task<Usluga?> GetUslugaByIdAsync(string id)
        {
            var usluga =
                uslugaList.FirstOrDefault(x => x.Id == id);

            return Task.FromResult(usluga);
        }

        public Task<RadnoVreme> GetRadnoVremeAsync()
        {
            return Task.FromResult(radnoVreme);
        }

        public Task SacuvajRadnoVremeAsync(RadnoVreme novoRadnoVreme)
        {
            radnoVreme.PocetakRadnogVremena =
                novoRadnoVreme.PocetakRadnogVremena;

            radnoVreme.KrajRadnogVremena =
                novoRadnoVreme.KrajRadnogVremena;

            radnoVreme.IntervalMinuti =
                novoRadnoVreme.IntervalMinuti;

            return Task.CompletedTask;
        }
    }
}