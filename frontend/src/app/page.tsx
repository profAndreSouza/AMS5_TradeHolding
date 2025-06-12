'use client';

import { useEffect, useState } from 'react';
import Header from '../components/common/Header';
import CryptoTable from '../components/home/CryptoTable';
import CryptoChart from '../components/home/CryptoChart';
import { getCoins, Coin } from '../services/cryptoCoinsService';
import { getChartData, ChartPoint } from '../services/cryptoChartService';

export default function Home() {

  const [coins, setCoins] = useState<Coin[]>([]);
  const [chartData, setChartData] = useState<ChartPoint[]>([]);
  const [selectedCoinId, setSelectedCoinId] = useState<string>('');
  const [selectedLimit, setSelectedLimit] = useState<number>(10);

  useEffect(() => {
    const fetchCoins  = async () => {
      const coinsData = await getCoins();
      setCoins(coinsData);
      if (coinsData.length > 0) {
        setSelectedCoinId(coinsData[0].id);
      }
    };

    fetchCoins ();
  }, []);

  useEffect(() => {
    if (selectedCoinId) {
      const fetchChart = async () => {
        const data = await getChartData(selectedCoinId, selectedLimit);
        setChartData(data);
      };
      fetchChart();
    }
  }, [selectedCoinId, selectedLimit]);

  return (
    <>
      <Header pageName="Página Inicial" />
      <main className="bg-black text-white min-h-screen p-8">
        
        <section className="py-16 text-center px-4">
          <h1 className="text-4xl md:text-6xl font-bold mb-4">
            Bem-vindo ao Trade Holding AMS
          </h1>
          <p className="text-lg md:text-2xl text-gray-300 max-w-2xl mx-auto">
            Acompanhe as principais criptomoedas em tempo real, visualize gráficos e fique por dentro das tendências do mercado.
          </p>
        </section>

        <section className="flex gap-4 h-5">
          <div className="w-1/3 h-full min-h-64">
            <CryptoTable coins={coins} />
          </div>
          <div className="w-2/3 h-full">
        <CryptoChart
          data={chartData}
          coinName={coins.find((c) => c.id === selectedCoinId)?.name || 'Carregando...'}
          coins={coins}
          onCoinChange={setSelectedCoinId}
          onLimitChange={setSelectedLimit}
        />
      </div>
        </section>
      </main>
    </>
  );
}
