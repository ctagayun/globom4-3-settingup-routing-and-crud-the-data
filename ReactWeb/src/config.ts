const config = {
    baseApiUrl: "https://localhost:4000",  //base url of the API
  };
  
  const currencyFormatter = Intl.NumberFormat("en-US", {
    style: "currency",
    currency: "USD",
    maximumFractionDigits: 0,
  });
  
  export default config;
  
  export { currencyFormatter };