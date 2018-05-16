import * as React from "react";
import { Route } from "react-router-dom";
import { Layout } from "./components/layout/Layout";
import About from "./components/About";
import DiceStatistics from "./components/DiceStatistics";

export const routes = <Layout>
	<Route exact path="/" component={DiceStatistics} />
	<Route path="/About" component={About} />
</Layout>;
