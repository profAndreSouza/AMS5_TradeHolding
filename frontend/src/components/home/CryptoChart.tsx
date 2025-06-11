// components/CryptoChart.tsx
import { LineChart, Line, XAxis, YAxis, Tooltip, ResponsiveContainer } from 'recharts';

type Props = {
  data: { time: string; value: number }[];
  coinName: string;
};

export default function CryptoChart({ data, coinName }: Props) {
  return (
    <div className="px-4 rounded-lg">
      <h2 className="text-lg font-bold mb-4">Variação de {coinName}</h2>
      <ResponsiveContainer width="100%" height={400}>
        <LineChart data={data}>
          <XAxis dataKey="time" stroke="#ccc" />
          <YAxis stroke="#ccc" />
          <Tooltip />
          <Line type="monotone" dataKey="value" stroke="#10b981" strokeWidth={2} />
        </LineChart>
      </ResponsiveContainer>
    </div>
  );
}
