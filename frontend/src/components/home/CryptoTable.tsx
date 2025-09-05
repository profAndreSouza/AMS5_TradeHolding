// components/CryptoTable.tsx
import React from 'react';

type Crypto = {
  symbol: string;
  name: string;
  price: number;
  change: number;
};

type Props = {
  coins: Crypto[];
};

export default function CryptoTable({ coins }: Props) {
  return (
    <div className="overflow-x-auto">
      <table className="px-4 min-w-full">
        <thead>
          <tr>
            <th className="p-2 text-left">Icone</th>
            <th className="p-2 text-left">Nome</th>
            <th className="p-2 text-left">Preço</th>
            <th className="p-2 text-left">Valorização</th>
          </tr>
        </thead>
        <tbody>
          {coins.map((coin) => (
            <tr key={coin.symbol} className="border-t border-gray-700">
              <td className="p-2">
                <img
                  src={`/coin/${coin.symbol}.svg`}
                  alt={`${coin.name} icon`}
                  className="w-6 h-6"
                />
              </td>
              <td className="p-2">{coin.name}</td>
              <td className="p-2">
                {coin.price.toLocaleString('pt-BR', {
                  style: 'currency',
                  currency: 'USD',
                  minimumFractionDigits: 4,
                  maximumFractionDigits: 4,
                })}
              </td>
              <td className={`p-2 ${coin.change >= 0 ? 'text-green-500' : 'text-red-500'}`}>
                {coin.change.toFixed(2)}%
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
