'use client';

import { useEffect, useState } from 'react';
import { LineChart, Line, XAxis, YAxis, Tooltip, ResponsiveContainer } from 'recharts';

type CryptoChartProps = {
  data: ChartPoint[];
  coinName: string;
  coins: Coin[];
  onCoinChange: (coinId: string) => void;
  onLimitChange: (limit: number) => void;
};

export default function CryptoChart({ data, coinName, coins, onCoinChange, onLimitChange }: CryptoChartProps) {
  const [selectedCoin, setSelectedCoin] = useState<string>(coins[0]?.id || '');
  const [selectedLimit, setSelectedLimit] = useState<number>(10);

  useEffect(() => {
    if (coins.length > 0) {
      setSelectedCoin(coins[0].id);
      onCoinChange(coins[0].id);
    }
  }, [coins]);

  const handleCoinChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const coinId = e.target.value;
    setSelectedCoin(coinId);
    onCoinChange(coinId);
  };

  const handleLimitChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const limit = Number(e.target.value);
    setSelectedLimit(limit);
    onLimitChange(limit);
  };

  return (
    <div className="px-4 rounded-lg">
      <h2 className="text-lg font-bold mb-4">Variação de {coinName}</h2>

      <div className="mb-4 flex gap-4 items-center">
        <label className="text-sm font-medium text-gray-300">
          Moeda:{' '}
          <select value={selectedCoin} onChange={handleCoinChange} className="bg-gray-800 text-white p-1 rounded">
            {coins.map((coin) => (
              <option key={coin.id} value={coin.id}>
                {coin.name}
              </option>
            ))}
          </select>
        </label>

        <label className="text-sm font-medium text-gray-300">
          Registros:{' '}
          <select value={selectedLimit} onChange={handleLimitChange} className="bg-gray-800 text-white p-1 rounded">
            {[10, 20, 30, 40, 50].map((n) => (
              <option key={n} value={n}>
                {n}
              </option>
            ))}
          </select>
        </label>
      </div>
      
      <ResponsiveContainer width="100%" height={400}>
        <LineChart data={data}>
          <XAxis  dataKey="time" stroke="#ccc" />
          <YAxis  stroke="#ccc" 
                  domain={([dataMin, dataMax]) => {
                    const margin = (dataMax - dataMin) * 0.1;
                    return [dataMin - margin, dataMax + margin];
                  }}
          />
          <Tooltip />
          <Line type="monotone" dataKey="value" stroke="#10b981" strokeWidth={2} />
        </LineChart>
      </ResponsiveContainer>

    </div>
  );
}
