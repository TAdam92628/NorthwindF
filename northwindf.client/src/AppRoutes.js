import { SumSup } from "./components/SumSup";
import { FetchProducts } from "./components/FetchProducts";
import { Home } from "./components/Home";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/sum-sup',
      element: <SumSup />
  },
  {
    path: '/fetch-products',
      element: <FetchProducts />
  }
];

export default AppRoutes;
