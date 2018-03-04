import * as React from 'react';
import { NavLink, Link } from 'react-router-dom';

export class NavMenu extends React.Component<{}, {}> {
	public render() {
		return <nav className="navbar navbar-inverse navbar-full">
			<div className="container-fluid">
				<div className="navbar-header">
					<button type="button" className="navbar-toggle collapsed" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1" aria-expanded="false">
						<span className="sr-only">Toggle navigation</span>
						<span className="icon-bar"></span>
						<span className="icon-bar"></span>
						<span className="icon-bar"></span>
					</button>
					<Link className='navbar-brand' to={'/'}>Visualizer</Link>
				</div>

				<div className="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
					<ul className="nav navbar-nav">
						<li id="ProjectBlock" className="dropdown">
							<NavLink to={'/DiceStatistics'} activeClassName='active'>
								<span className='glyphicon glyphicon-education'></span> Dice Statistics
                            </NavLink>
						</li>
					</ul>
				</div>
			</div>
		</nav>;
	}
}
