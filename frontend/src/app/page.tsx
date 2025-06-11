'use client';
import Header from '../components/common/Header';
import CryptoTable from '../components/home/CryptoTable';
import CryptoChart from '../components/home/CryptoChart';

  const coins = [
    { id: 'BTC', name: 'Bitcoin', price: 67000, change: 2.3 },
    { id: 'ETH', name: 'Ethereum', price: 3700, change: -1.2 },
    { id: 'BNB', name: 'Binance Coin', price: 580, change: 0.5 },
    { id: 'ADA', name: 'Cardano', price: 0.55, change: -0.8 },
    { id: 'SOL', name: 'Solana', price: 150, change: 4.1 },
  ];

  const chartData = [
    { time: '09:00', value: 66000 },
    { time: '10:00', value: 66500 },
    { time: '11:00', value: 67000 },
    { time: '12:00', value: 67500 },
    { time: '13:00', value: 67300 },
  ];

export default function Home() {

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
            <CryptoChart data={chartData} coinName={coins[0].name} />
          </div>
        </section>
      </main>
    </>
  );
}
