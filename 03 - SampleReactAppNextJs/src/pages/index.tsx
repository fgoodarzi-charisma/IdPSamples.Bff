import { Inter } from "next/font/google";
import styles from "@/styles/Home.module.css";
import { useEffect, useState } from "react";
import { useRouter } from "next/router";

const inter = Inter({ subsets: ["latin"] });

export default function Home() {
  const router = useRouter();
  const [state, setState] = useState([]);

  useEffect(() => {
    fetch("/bff/user").then(async (rs) => {
      if (rs.status === 401) {
        router.push("/bff/login");
      } else {
        const json = await rs.json();
        setState(json);
      }
      
    });
  }, []);

  return (
    <>
      <main className={`${styles.main} ${inter.className}`}>
        <pre>
          {JSON.stringify(state, null, "\t")}
        </pre>
      </main>
    </>
  );
}
